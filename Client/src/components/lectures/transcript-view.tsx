"use client";

import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { ScrollArea } from "@/components/ui/scroll-area";
import { FileText } from "lucide-react";
import { useEffect, useRef, useState } from "react";
import { Badge } from "../ui/badge";
import { TranscriptionDto } from "@/api/web-api-client";
import { useMutation } from "@tanstack/react-query";
import useApi from "@/api/use-api";
import { Button } from "../ui/button";
import { LoadingButton } from "../shared/loading-button";

interface TranscriptViewProps {
  transcription: TranscriptionDto;
  highlightKeyword?: string | null; // Added optional prop for external keyword highlighting
}

export function TranscriptView({
  transcription,
  highlightKeyword,
}: TranscriptViewProps) {
  const { lecturesClient } = useApi();
  const transcriptRef = useRef<HTMLDivElement>(null);
  const [highlightedTerm, setHighlightedTerm] = useState<string | null>(null);

  const keywords = useMutation({
    mutationFn: () =>
      lecturesClient.extractTranscriptKeywords(transcription.id),
  });

  console.log(transcription);
  useEffect(() => {
    if (highlightKeyword) {
      setHighlightedTerm(highlightKeyword);

      // Scroll to first occurrence
      if (transcriptRef.current) {
        const element = transcriptRef.current.querySelector(
          `[data-term="${highlightKeyword.toLowerCase()}"]`
        );
        if (element) {
          element.scrollIntoView({ behavior: "smooth", block: "center" });
        }
      }

      // Clear highlight after 3 seconds
      setTimeout(() => setHighlightedTerm(null), 3000);
    }
  }, [highlightKeyword]);

  useEffect(() => {
    const handleKeywordClick = (event: CustomEvent) => {
      const { term } = event.detail;
      setHighlightedTerm(term);

      // Clear highlight after 3 seconds
      setTimeout(() => setHighlightedTerm(null), 3000);

      // Scroll to first occurrence
      if (transcriptRef.current) {
        const element = transcriptRef.current.querySelector(
          `[data-term="${term.toLowerCase()}"]`
        );
        if (element) {
          element.scrollIntoView({ behavior: "smooth", block: "center" });
        }
      }
    };

    window.addEventListener("keyword-click" as any, handleKeywordClick);
    return () =>
      window.removeEventListener("keyword-click" as any, handleKeywordClick);
  }, []);

  // // Split transcript into paragraphs and highlight keywords

  // const highlightKeywords = (text: string) => {
  //   const result = text
  //   const parts: React.ReactNode[] = []
  //   let lastIndex = 0

  //   // Find all keyword occurrences
  //   lecture.keywords.forEach((keyword) => {
  //     const regex = new RegExp(`\\b(${keyword.term})\\b`, "gi")
  //     let match

  //     while ((match = regex.exec(text)) !== null) {
  //       // Add text before match
  //       if (match.index > lastIndex) {
  //         parts.push(text.substring(lastIndex, match.index))
  //       }

  //       // Add highlighted keyword
  //       const isHighlighted = highlightedTerm?.toLowerCase() === keyword.term.toLowerCase()
  //       parts.push(
  //         <span
  //           key={`${keyword.term}-${match.index}`}
  //           data-term={keyword.term.toLowerCase()}
  //           className={`font-medium transition-colors ${
  //             isHighlighted
  //               ? "bg-accent text-accent-foreground px-1 rounded"
  //               : "text-primary hover:text-primary/80 cursor-pointer"
  //           }`}
  //         >
  //           {match[0]}
  //         </span>,
  //       )

  //       lastIndex = match.index + match[0].length
  //     }
  //   })

  //   // Add remaining text
  //   if (lastIndex < text.length) {
  //     parts.push(text.substring(lastIndex))
  //   }

  //   return
  // }

  return (
    <Card className="h-fit">
      <CardHeader>
        <CardTitle className="flex items-center gap-2">
          <FileText className="h-5 w-5" />
          Transcript
          <Badge variant="secondary" className="ml-2">
            {transcription.language}
          </Badge>
          <LoadingButton
            variant="outline"
            size="sm"
            onClick={() => keywords.mutate()}
            loading={keywords.isPending}
          >
            Extract Keywords
          </LoadingButton>
        </CardTitle>
      </CardHeader>
      <CardContent>
        <ScrollArea className="h-[600px] pr-4">
          <div ref={transcriptRef} className="space-y-4">
            {transcription.items.map((paragraph, index) => (
              <p
                key={index}
                className="text-sm leading-relaxed text-foreground"
              >
                {paragraph.text}
              </p>
            ))}
          </div>
        </ScrollArea>
      </CardContent>
    </Card>
  );
}
