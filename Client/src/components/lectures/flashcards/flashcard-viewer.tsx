"use client"

import { useState } from "react"
import { Card } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { ChevronLeft, ChevronRight, RotateCcw, Sparkles } from "lucide-react"
import type { Flashcard } from "@/lib/types"

interface FlashcardViewerProps {
  flashcards: Flashcard[]
  onKeywordClick?: (keyword: string) => void
}

export function FlashcardViewer({ flashcards, onKeywordClick }: FlashcardViewerProps) {
  const [currentIndex, setCurrentIndex] = useState(0)
  const [isFlipped, setIsFlipped] = useState(false)
  const [studiedCards, setStudiedCards] = useState<Set<number>>(new Set())

  const currentCard = flashcards[currentIndex]

  const handleNext = () => {
    setIsFlipped(false)
    setStudiedCards((prev) => new Set(prev).add(currentIndex))
    setCurrentIndex((prev) => (prev + 1) % flashcards.length)
  }

  const handlePrevious = () => {
    setIsFlipped(false)
    setCurrentIndex((prev) => (prev - 1 + flashcards.length) % flashcards.length)
  }

  const handleFlip = () => {
    setIsFlipped(!isFlipped)
    if (!isFlipped) {
      setStudiedCards((prev) => new Set(prev).add(currentIndex))
    }
  }

  const handleReset = () => {
    setCurrentIndex(0)
    setIsFlipped(false)
    setStudiedCards(new Set())
  }

  const progress = (studiedCards.size / flashcards.length) * 100

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="space-y-1">
          <h3 className="text-lg font-semibold text-foreground">Study Flashcards</h3>
          <p className="text-sm text-muted-foreground">
            {studiedCards.size} of {flashcards.length} cards studied
          </p>
        </div>
        <Button variant="outline" size="sm" onClick={handleReset}>
          <RotateCcw className="h-4 w-4 mr-2" />
          Reset Progress
        </Button>
      </div>

      <div className="w-full bg-muted rounded-full h-2">
        <div className="bg-primary h-2 rounded-full transition-all duration-300" style={{ width: `${progress}%` }} />
      </div>

      <div className="relative perspective-1000">
        <Card
          className="relative h-80 cursor-pointer transition-all duration-500 preserve-3d"
          style={{
            transformStyle: "preserve-3d",
            transform: isFlipped ? "rotateY(180deg)" : "rotateY(0deg)",
          }}
          onClick={handleFlip}
        >
          <div
            className="absolute inset-0 p-8 flex flex-col items-center justify-center backface-hidden"
            style={{ backfaceVisibility: "hidden" }}
          >
            <div className="flex items-center gap-2 mb-4">
              <Sparkles className="h-5 w-5 text-primary" />
              <Badge variant="secondary">{currentCard.category}</Badge>
            </div>
            <p className="text-xl font-medium text-center text-foreground">{currentCard.question}</p>
            <p className="text-sm text-muted-foreground mt-6">Click to reveal answer</p>
          </div>

          <div
            className="absolute inset-0 p-8 flex flex-col items-center justify-center backface-hidden"
            style={{
              backfaceVisibility: "hidden",
              transform: "rotateY(180deg)",
            }}
          >
            <Badge variant="secondary" className="mb-4">
              {currentCard.category}
            </Badge>
            <p className="text-base text-center text-foreground leading-relaxed">{currentCard.answer}</p>
            {currentCard.relatedKeyword && onKeywordClick && (
              <Button
                variant="link"
                size="sm"
                className="mt-6"
                onClick={(e) => {
                  e.stopPropagation()
                  onKeywordClick(currentCard.relatedKeyword!)
                }}
              >
                View in transcript: {currentCard.relatedKeyword}
              </Button>
            )}
          </div>
        </Card>
      </div>

      <div className="flex items-center justify-between">
        <Button variant="outline" onClick={handlePrevious} disabled={flashcards.length <= 1}>
          <ChevronLeft className="h-4 w-4 mr-2" />
          Previous
        </Button>

        <span className="text-sm text-muted-foreground">
          {currentIndex + 1} / {flashcards.length}
        </span>

        <Button variant="outline" onClick={handleNext} disabled={flashcards.length <= 1}>
          Next
          <ChevronRight className="h-4 w-4 ml-2" />
        </Button>
      </div>
    </div>
  )
}
