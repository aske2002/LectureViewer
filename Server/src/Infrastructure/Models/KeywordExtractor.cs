using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

public static class KeywordCandidateExtractor
{
    // Unicode-aware tokenization (any language)
    private static readonly Regex TokenRegex =
        new(@"[\p{L}\p{N}]+", RegexOptions.Compiled);

    // URLs, timestamps, punctuation burst normalization
    private static readonly Regex UrlRegex =
        new(@"https?://\S+|www\.\S+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Regex TimestampRegex =
        new(@"\b\d{1,2}:\d{2}(:\d{2})?\b", RegexOptions.Compiled);

    private static readonly Regex MultiPunctRegex =
        new(@"[\p{P}]+", RegexOptions.Compiled);

    // --- PUBLIC API ---
    public static List<string> ExtractCandidatePhrases(
        string text, int maxCandidates = 400)
    {
        if (string.IsNullOrWhiteSpace(text))
            return new();

        // 1. Clean raw transcript noise
        text = Preclean(text);

        // 2. Tokenize
        var tokens = Tokenize(text);
        if (tokens.Count == 0)
            return new();

        // 3. Detect dynamic stopwords using entropy & frequency
        var stopwords = ComputeDynamicStopwords(tokens);

        // 4. Generate N-grams (1–5)
        var ngrams = GenerateNgrams(tokens, stopwords, maxN: 5);

        // 5. Compute PMI-like scores for phrases
        var scored = ScorePhrases(tokens, ngrams);

        // 6. Filter by script consistency (no language-mixing inside phrase)
        var filtered = scored
            .Where(kvp => IsScriptConsistent(kvp.Key))
            .ToList();

        // 7. Frequency-length final heuristic
        var final = filtered
            .OrderByDescending(kvp => kvp.Value)
            .Select(kvp => kvp.Key)
            .Distinct()
            .Take(maxCandidates)
            .ToList();

        return final;
    }


    // ------------------------------------------------------------
    //  CLEANING
    // ------------------------------------------------------------
    private static string Preclean(string text)
    {
        text = UrlRegex.Replace(text, " ");
        text = TimestampRegex.Replace(text, " ");

        // Normalize punctuation bursts into spaced tokens
        text = MultiPunctRegex.Replace(text, m => $" {m.Value} ");

        // Collapse whitespace
        text = Regex.Replace(text, @"\s+", " ");

        return text.Trim();
    }


    // ------------------------------------------------------------
    //  TOKENIZATION
    // ------------------------------------------------------------
    private static List<string> Tokenize(string text)
    {
        return TokenRegex
            .Matches(text)
            .Select(m => m.Value)
            .Where(t => t.Length > 1)   // remove tiny noise tokens
            .ToList();
    }


    // ------------------------------------------------------------
    //  DYNAMIC STOPWORDS
    // ------------------------------------------------------------
    private static HashSet<string> ComputeDynamicStopwords(List<string> tokens)
    {
        var freq = new Dictionary<string, int>();
        for (int i = 0; i < tokens.Count; i++)
        {
            var t = tokens[i];
            freq[t] = freq.GetValueOrDefault(t) + 1;
        }

        int total = tokens.Count;

        // A token is a stopword if:
        // 1. occurs frequently AND
        // 2. has high positional entropy (appears uniformly everywhere)
        var stopwords = new HashSet<string>();

        foreach (var kvp in freq)
        {
            string token = kvp.Key;
            int f = kvp.Value;

            if (f < 4) continue; // cannot be a stopword if too rare

            double p = (double)f / total;

            // high-frequency tokens: p > ~2%
            if (p < 0.02) continue;

            // approximate entropy: if token appears uniformly → high randomness → stopword
            double entropy = -p * Math.Log(p, 2);

            if (entropy > 0.8) // threshold tuned empirically
                stopwords.Add(token);
        }

        return stopwords;
    }


    // ------------------------------------------------------------
    //  N-GRAM GENERATION WITH STOPWORD BOUNDARIES
    // ------------------------------------------------------------
    private static List<string> GenerateNgrams(
        List<string> tokens, HashSet<string> stopwords, int maxN)
    {
        var results = new List<string>();
        int nTokens = tokens.Count;

        for (int i = 0; i < nTokens; i++)
        {
            if (stopwords.Contains(tokens[i])) continue;

            for (int n = 1; n <= maxN && i + n <= nTokens; n++)
            {
                bool broken = false;
                for (int j = 0; j < n; j++)
                {
                    if (stopwords.Contains(tokens[i + j]))
                    {
                        broken = true;
                        break;
                    }
                }
                if (broken) break;

                var phraseTokens = tokens.GetRange(i, n);
                var phrase = string.Join(" ", phraseTokens);

                if (phrase.Length > 3 && phrase.Length < 60)
                    results.Add(phrase);
            }
        }

        return results;
    }


    // ------------------------------------------------------------
    //  PMI-LIKE PHRASE SCORING
    // ------------------------------------------------------------
    private static Dictionary<string, float> ScorePhrases(
        List<string> tokens, List<string> phrases)
    {
        var tokenFreq = tokens
            .GroupBy(t => t)
            .ToDictionary(g => g.Key, g => g.Count());

        var phraseFreq = phrases
            .GroupBy(p => p)
            .ToDictionary(g => g.Key, g => g.Count());

        var scores = new Dictionary<string, float>();

        foreach (var kvp in phraseFreq)
        {
            string phrase = kvp.Key;
            int fPhrase = kvp.Value;

            var words = phrase.Split(' ');
            if (words.Length == 0) continue;

            float productFreq = 1;
            foreach (var w in words)
                productFreq *= tokenFreq.GetValueOrDefault(w, 1);

            float cohesion = fPhrase / (float)Math.Pow(productFreq, 0.5);

            // reward longer phrases slightly
            cohesion *= (1 + 0.3f * (words.Length - 1));

            scores[phrase] = cohesion;
        }

        return scores;
    }


    // ------------------------------------------------------------
    //  SCRIPT CONSISTENCY FILTER
    // ------------------------------------------------------------
    private static bool IsScriptConsistent(string phrase)
    {
        string[] tokens = phrase.Split(' ');
        int? baseClass = null;

        foreach (var t in tokens)
        {
            int cls = GetScriptClass(t);

            if (cls == -1) return false;

            if (baseClass == null)
                baseClass = cls;
            else if (baseClass != cls)
                return false;
        }

        return true;
    }

    // Return simple script class ID:
    // Latin=1, Cyrillic=2, Arabic=3, Han=4, Devanagari=5, Kana=6, Other=-1
    private static int GetScriptClass(string token)
    {
        foreach (char c in token)
        {
            int cls = char.GetUnicodeCategory(c) switch
            {
                UnicodeCategory.UppercaseLetter or
                UnicodeCategory.LowercaseLetter or
                UnicodeCategory.TitlecaseLetter or
                UnicodeCategory.ModifierLetter or
                UnicodeCategory.OtherLetter => ScriptOf(c),
                _ => -1
            };

            if (cls == -1) return -1;
        }
        return 0;
    }

    private static int ScriptOf(char c)
    {
        int code = c;

        if (code <= 0x024F) return 1;                 // Latin + extended
        if (code is >= 0x0400 and <= 0x04FF) return 2; // Cyrillic
        if (code is >= 0x0600 and <= 0x06FF) return 3; // Arabic
        if (code is >= 0x4E00 and <= 0x9FFF) return 4; // Han (Chinese)
        if (code is >= 0x0900 and <= 0x097F) return 5; // Devanagari
        if (code is >= 0x3040 and <= 0x30FF) return 6; // Hiragana/Katakana

        return 1; // default to Latin-like unless unsure
    }
}
