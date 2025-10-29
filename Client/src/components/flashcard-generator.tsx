"use client"

import { useState } from "react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Sparkles, Loader2, CheckCircle } from "lucide-react"
import type { Lecture } from "@/lib/types"

interface FlashcardGeneratorProps {
  lecture: Lecture
  onGenerate?: () => void
}

export function FlashcardGenerator({ lecture, onGenerate }: FlashcardGeneratorProps) {
  const [isGenerating, setIsGenerating] = useState(false)
  const [isComplete, setIsComplete] = useState(false)

  const handleGenerate = async () => {
    setIsGenerating(true)
    setIsComplete(false)

    // Simulate AI generation (in real app, this would call an API)
    await new Promise((resolve) => setTimeout(resolve, 2000))

    setIsGenerating(false)
    setIsComplete(true)
    onGenerate?.()

    // Reset complete state after 3 seconds
    setTimeout(() => setIsComplete(false), 3000)
  }

  return (
    <Card className="border-dashed">
      <CardHeader>
        <CardTitle className="flex items-center gap-2 text-lg">
          <Sparkles className="h-5 w-5 text-primary" />
          Generate More Flashcards
        </CardTitle>
        <CardDescription>
          Use AI to automatically generate additional flashcards from the lecture transcript and keywords.
        </CardDescription>
      </CardHeader>
      <CardContent>
        <Button onClick={handleGenerate} disabled={isGenerating || isComplete} className="w-full">
          {isGenerating && <Loader2 className="h-4 w-4 mr-2 animate-spin" />}
          {isComplete && <CheckCircle className="h-4 w-4 mr-2" />}
          {isGenerating ? "Generating..." : isComplete ? "Generated!" : "Generate Flashcards"}
        </Button>
      </CardContent>
    </Card>
  )
}
