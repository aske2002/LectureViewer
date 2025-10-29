"use client"

import { useState } from "react"
import { Card } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Search, BookOpen } from "lucide-react"
import type { Flashcard } from "@/lib/types"

interface FlashcardListProps {
  flashcards: Flashcard[]
  onCardClick?: (flashcard: Flashcard) => void
}

export function FlashcardList({ flashcards, onCardClick }: FlashcardListProps) {
  const [searchQuery, setSearchQuery] = useState("")

  const filteredCards = flashcards.filter(
    (card) =>
      card.question.toLowerCase().includes(searchQuery.toLowerCase()) ||
      card.answer.toLowerCase().includes(searchQuery.toLowerCase()) ||
      card.category.toLowerCase().includes(searchQuery.toLowerCase()),
  )

  const categories = Array.from(new Set(flashcards.map((card) => card.category)))

  return (
    <div className="space-y-6">
      <div className="flex items-center gap-4">
        <div className="relative flex-1">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
          <Input
            placeholder="Search flashcards..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            className="pl-10"
          />
        </div>
      </div>

      <div className="flex flex-wrap gap-2">
        {categories.map((category) => (
          <Badge key={category} variant="outline" className="cursor-pointer hover:bg-accent">
            {category}
          </Badge>
        ))}
      </div>

      <div className="space-y-3">
        {filteredCards.length === 0 ? (
          <Card className="p-8 text-center">
            <BookOpen className="h-12 w-12 mx-auto mb-4 text-muted-foreground" />
            <p className="text-muted-foreground">No flashcards found matching your search.</p>
          </Card>
        ) : (
          filteredCards.map((card) => (
            <Card
              key={card.id}
              className="p-4 hover:bg-accent/50 transition-colors cursor-pointer"
              onClick={() => onCardClick?.(card)}
            >
              <div className="flex items-start justify-between gap-4">
                <div className="flex-1 space-y-2">
                  <div className="flex items-center gap-2">
                    <Badge variant="secondary" className="text-xs">
                      {card.category}
                    </Badge>
                    {card.relatedKeyword && (
                      <Badge variant="outline" className="text-xs">
                        {card.relatedKeyword}
                      </Badge>
                    )}
                  </div>
                  <p className="font-medium text-foreground">{card.question}</p>
                  <p className="text-sm text-muted-foreground line-clamp-2">{card.answer}</p>
                </div>
                <Button variant="ghost" size="sm">
                  View
                </Button>
              </div>
            </Card>
          ))
        )}
      </div>
    </div>
  )
}
