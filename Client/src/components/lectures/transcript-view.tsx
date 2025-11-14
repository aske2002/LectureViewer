"use client"

import type React from "react"

import type { Lecture } from "@/lib/types"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { ScrollArea } from "@/components/ui/scroll-area"
import { FileText } from "lucide-react"
import { useEffect, useRef, useState } from "react"

interface TranscriptViewProps {
  lecture: Lecture
  highlightKeyword?: string | null // Added optional prop for external keyword highlighting
}

export function TranscriptView({ lecture, highlightKeyword }: TranscriptViewProps) {
  const transcriptRef = useRef<HTMLDivElement>(null)
  const [highlightedTerm, setHighlightedTerm] = useState<string | null>(null)

  useEffect(() => {
    if (highlightKeyword) {
      setHighlightedTerm(highlightKeyword)

      // Scroll to first occurrence
      if (transcriptRef.current) {
        const element = transcriptRef.current.querySelector(`[data-term="${highlightKeyword.toLowerCase()}"]`)
        if (element) {
          element.scrollIntoView({ behavior: "smooth", block: "center" })
        }
      }

      // Clear highlight after 3 seconds
      setTimeout(() => setHighlightedTerm(null), 3000)
    }
  }, [highlightKeyword])

  useEffect(() => {
    const handleKeywordClick = (event: CustomEvent) => {
      const { term } = event.detail
      setHighlightedTerm(term)

      // Clear highlight after 3 seconds
      setTimeout(() => setHighlightedTerm(null), 3000)

      // Scroll to first occurrence
      if (transcriptRef.current) {
        const element = transcriptRef.current.querySelector(`[data-term="${term.toLowerCase()}"]`)
        if (element) {
          element.scrollIntoView({ behavior: "smooth", block: "center" })
        }
      }
    }

    window.addEventListener("keyword-click" as any, handleKeywordClick)
    return () => window.removeEventListener("keyword-click" as any, handleKeywordClick)
  }, [])

  // Split transcript into paragraphs and highlight keywords
  const paragraphs = lecture.transcript.split("\n\n").filter((p) => p.trim())

  const highlightKeywords = (text: string) => {
    const result = text
    const parts: React.ReactNode[] = []
    let lastIndex = 0

    // Find all keyword occurrences
    lecture.keywords.forEach((keyword) => {
      const regex = new RegExp(`\\b(${keyword.term})\\b`, "gi")
      let match

      while ((match = regex.exec(text)) !== null) {
        // Add text before match
        if (match.index > lastIndex) {
          parts.push(text.substring(lastIndex, match.index))
        }

        // Add highlighted keyword
        const isHighlighted = highlightedTerm?.toLowerCase() === keyword.term.toLowerCase()
        parts.push(
          <span
            key={`${keyword.term}-${match.index}`}
            data-term={keyword.term.toLowerCase()}
            className={`font-medium transition-colors ${
              isHighlighted
                ? "bg-accent text-accent-foreground px-1 rounded"
                : "text-primary hover:text-primary/80 cursor-pointer"
            }`}
          >
            {match[0]}
          </span>,
        )

        lastIndex = match.index + match[0].length
      }
    })

    // Add remaining text
    if (lastIndex < text.length) {
      parts.push(text.substring(lastIndex))
    }

    return parts.length > 0 ? parts : text
  }

  return (
    <Card className="h-fit">
      <CardHeader>
        <CardTitle className="flex items-center gap-2">
          <FileText className="h-5 w-5" />
          Transcript
        </CardTitle>
      </CardHeader>
      <CardContent>
        <ScrollArea className="h-[600px] pr-4">
          <div ref={transcriptRef} className="space-y-4">
            {paragraphs.map((paragraph, index) => (
              <p key={index} className="text-sm leading-relaxed text-foreground">
                {highlightKeywords(paragraph)}
              </p>
            ))}
          </div>
        </ScrollArea>
      </CardContent>
    </Card>
  )
}
