import type { Flashcard, Lecture } from "./types"

/**
 * Generates flashcards from lecture content
 * In a real application, this would use AI to generate questions and answers
 * For now, it creates flashcards based on keywords and transcript context
 */
export function generateFlashcards(lecture: Lecture): Flashcard[] {
  const flashcards: Flashcard[] = []

  // Generate flashcards from keywords
  lecture.keywords.forEach((keyword, index) => {
    // Get context from the first occurrence
    const firstOccurrence = keyword.occurrences[0]

    // Extract sentences around the keyword from transcript
    const sentences = lecture.transcript.split(/[.!?]+/).filter((s) => s.trim())
    const relevantSentence = sentences.find((s) => s.toLowerCase().includes(keyword.term.toLowerCase()))

    if (relevantSentence) {
      flashcards.push({
        id: `${lecture.id}-${index + 1}`,
        question: `What is ${keyword.term}?`,
        answer: relevantSentence.trim() + ".",
        category: "Key Concepts",
        relatedKeyword: keyword.term,
      })
    }
  })

  // Generate definition-style flashcards
  const definitionPatterns = [
    /(\w+(?:\s+\w+)*)\s+is\s+(?:a|an|the)\s+([^.!?]+[.!?])/gi,
    /(\w+(?:\s+\w+)*)\s+are\s+([^.!?]+[.!?])/gi,
  ]

  definitionPatterns.forEach((pattern) => {
    let match
    while ((match = pattern.exec(lecture.transcript)) !== null) {
      const term = match[1].trim()
      const definition = match[2].trim()

      // Only add if not already covered by keywords
      if (!flashcards.some((f) => f.relatedKeyword?.toLowerCase() === term.toLowerCase())) {
        flashcards.push({
          id: `${lecture.id}-def-${flashcards.length + 1}`,
          question: `What ${match[0].includes(" are ") ? "are" : "is"} ${term}?`,
          answer: `${term.charAt(0).toUpperCase() + term.slice(1)} ${match[0].includes(" are ") ? "are" : "is"} ${definition}`,
          category: "Definitions",
        })
      }
    }
  })

  return flashcards
}

/**
 * Categorizes flashcards based on content analysis
 */
export function categorizeFlashcards(flashcards: Flashcard[]): Map<string, Flashcard[]> {
  const categories = new Map<string, Flashcard[]>()

  flashcards.forEach((card) => {
    const category = card.category || "Uncategorized"
    if (!categories.has(category)) {
      categories.set(category, [])
    }
    categories.get(category)!.push(card)
  })

  return categories
}

/**
 * Filters flashcards by search query
 */
export function searchFlashcards(flashcards: Flashcard[], query: string): Flashcard[] {
  const lowerQuery = query.toLowerCase()
  return flashcards.filter(
    (card) =>
      card.question.toLowerCase().includes(lowerQuery) ||
      card.answer.toLowerCase().includes(lowerQuery) ||
      card.category.toLowerCase().includes(lowerQuery) ||
      card.relatedKeyword?.toLowerCase().includes(lowerQuery),
  )
}
