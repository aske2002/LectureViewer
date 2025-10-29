export interface Flashcard {
  id: string
  question: string
  answer: string
  category: string
  relatedKeyword?: string
}

export interface Lecture {
  id: string
  name: string
  date: string
  duration: string
  transcript: string
  summary: string
  keywords: Keyword[]
  flashcards: Flashcard[]
  mediaUrl?: string
  mediaType?: "audio" | "video"
  thumbnailUrl?: string
}

export interface Keyword {
  term: string
  occurrences: KeywordOccurrence[]
}

export interface KeywordOccurrence {
  timestamp: string
  context: string
}

export interface SearchResult {
  lectureId: string
  lectureName: string
  matches: {
    type: "keyword" | "transcript"
    text: string
    timestamp?: string
    context: string
  }[]
}
