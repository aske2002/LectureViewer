import { z } from "zod";


const TimestampSchema = z.object({
  from: z.string(),
  to: z.string(),
});

const OffsetSchema = z.object({
  from: z.number(),
  to: z.number(),
});

const TokenSchema = z.object({
  text: z.string(),
  timestamps: TimestampSchema,
  offsets: OffsetSchema,
  id: z.number(),
  p: z.number(),
  t_dtw: z.number(),
});

const TranscriptionEntrySchema = z.object({
  timestamps: TimestampSchema,
  offsets: OffsetSchema,
  text: z.string(),
  tokens: z.array(TokenSchema),
});

const ModelSubConfigSchema = z.object({
  ctx: z.number(),
  state: z.number(),
  head: z.number(),
  layer: z.number(),
});

const ModelSchema = z.object({
  type: z.string(),
  multilingual: z.boolean(),
  vocab: z.number(),
  audio: ModelSubConfigSchema,
  text: ModelSubConfigSchema,
  mels: z.number(),
  ftype: z.number(),
});

const ParamsSchema = z.object({
  model: z.string(),
  language: z.string(),
  translate: z.boolean(),
});

const ResultSchema = z.object({
  language: z.string(),
});


export enum WhisperLanguage {
  Auto = "auto",
  Afrikaans = "af",
  Amharic = "am",
  Arabic = "ar",
  Assamese = "as",
  Azerbaijani = "az",
  Bashkir = "ba",
  Belarusian = "be",
  Bulgarian = "bg",
  Bengali = "bn",
  Tibetan = "bo",
  Breton = "br",
  Bosnian = "bs",
  Catalan = "ca",
  Czech = "cs",
  Welsh = "cy",
  Danish = "da",
  German = "de",
  Greek = "el",
  English = "en",
  Spanish = "es",
  Estonian = "et",
  Basque = "eu",
  Persian = "fa",
  Finnish = "fi",
  Faroese = "fo",
  French = "fr",
  Galician = "gl",
  Gujarati = "gu",
  Hausa = "ha",
  Hawaiian = "haw",
  Hebrew = "he",
  Hindi = "hi",
  Croatian = "hr",
  HaitianCreole = "ht",
  Hungarian = "hu",
  Armenian = "hy",
  Indonesian = "id",
  Icelandic = "is",
  Italian = "it",
  Japanese = "ja",
  Javanese = "jw",
  Georgian = "ka",
  Kazakh = "kk",
  Khmer = "km",
  Kannada = "kn",
  Korean = "ko",
  Latin = "la",
  Luxembourgish = "lb",
  Lingala = "ln",
  Lao = "lo",
  Lithuanian = "lt",
  Latvian = "lv",
  Malagasy = "mg",
  Maori = "mi",
  Macedonian = "mk",
  Malayalam = "ml",
  Mongolian = "mn",
  Marathi = "mr",
  Malay = "ms",
  Maltese = "mt",
  Burmese = "my",
  Nepali = "ne",
  Dutch = "nl",
  Nynorsk = "nn",
  Norwegian = "no",
  Occitan = "oc",
  Punjabi = "pa",
  Polish = "pl",
  Pashto = "ps",
  Portuguese = "pt",
  Romanian = "ro",
  Russian = "ru",
  Sanskrit = "sa",
  Sindhi = "sd",
  Sinhala = "si",
  Slovak = "sk",
  Slovenian = "sl",
  Shona = "sn",
  Somali = "so",
  Albanian = "sq",
  Serbian = "sr",
  Sundanese = "su",
  Swedish = "sv",
  Swahili = "sw",
  Tamil = "ta",
  Telugu = "te",
  Tajik = "tg",
  Thai = "th",
  Turkmen = "tk",
  Tagalog = "tl",
  Turkish = "tr",
  Tatar = "tt",
  Ukrainian = "uk",
  Urdu = "ur",
  Uzbek = "uz",
  Vietnamese = "vi",
  Yiddish = "yi",
  Yoruba = "yo",
  Cantonese = "yue",
  Chinese = "zh",
}

export enum WhisperModel {
  Tiny = "tiny",
  Base = "base",
  Small = "small",
  Medium = "medium",
  Large = "large",
  LargeV2 = "large-v2",
  LargeV3 = "large-v3",
}

export const WhisperOutputSchema = z.object({
  systeminfo: z.string(),
  model: ModelSchema,
  params: ParamsSchema,
  result: ResultSchema,
  transcription: z.array(TranscriptionEntrySchema),
});

export type WhisperOutput = z.infer<typeof WhisperOutputSchema>;
