"use client"


import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { ScrollArea } from "@/components/ui/scroll-area"
import { FileText } from "lucide-react"
import { useRef } from "react"

interface TranscriptSummaryViewProps {
  summary: string
}

export function TranscriptSummaryView({ summary }: TranscriptSummaryViewProps) {
  const transcriptRef = useRef<HTMLDivElement>(null)

  return (
    <Card className="h-fit">
      <CardHeader>
        <CardTitle className="flex items-center gap-2">
          <FileText className="h-5 w-5" />
          Transcript Summary
        </CardTitle>
      </CardHeader>
      <CardContent>
        <ScrollArea className="pr-4">
          <div ref={transcriptRef} className="space-y-4">
              {summary.split("\n").map((paragraph, index) => (
                <p key={index} className="text-sm leading-relaxed text-foreground">
                  {paragraph}
                </p>
              ))}
          </div>
        </ScrollArea>
      </CardContent>
    </Card>
  )
}
