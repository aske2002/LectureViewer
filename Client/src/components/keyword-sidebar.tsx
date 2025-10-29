"use client"

import type { Keyword } from "@/lib/types"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { ScrollArea } from "@/components/ui/scroll-area"
import { Hash, Clock, TrendingUp } from "lucide-react"
import { useState } from "react"

interface KeywordSidebarProps {
  keywords: Keyword[]
  lectureId: string
  expanded?: boolean // Added optional expanded prop for full-width view
}

export function KeywordSidebar({ keywords, expanded = false }: KeywordSidebarProps) {
  const [activeKeyword, setActiveKeyword] = useState<string | null>(null)

  const handleKeywordClick = (term: string, timestamp: string) => {
    setActiveKeyword(term)
    window.dispatchEvent(
      new CustomEvent("keyword-click", {
        detail: { term, timestamp },
      }),
    )
  }

  const sortedKeywords = [...keywords].sort((a, b) => b.occurrences.length - a.occurrences.length)

  return (
    <Card className={expanded ? "h-fit" : "h-fit lg:sticky lg:top-8"}>
      <CardHeader>
        <div className="flex items-center justify-between">
          <CardTitle className="flex items-center gap-2 text-lg">
            <Hash className="h-5 w-5" />
            Keywords
          </CardTitle>
          <Badge variant="outline" className="gap-1">
            <TrendingUp className="h-3 w-3" />
            {keywords.length}
          </Badge>
        </div>
      </CardHeader>
      <CardContent>
        <ScrollArea className={expanded ? "h-[800px] pr-4" : "h-[600px] pr-4"}>
          <div className={expanded ? "grid grid-cols-1 md:grid-cols-2 gap-4" : "space-y-4"}>
            {sortedKeywords.map((keyword) => (
              <div
                key={keyword.term}
                className={`space-y-2 p-3 rounded-lg transition-colors ${
                  activeKeyword === keyword.term ? "bg-muted" : ""
                }`}
              >
                <div className="flex items-center justify-between gap-2">
                  <h3 className="font-medium text-sm text-balance">{keyword.term}</h3>
                  <Badge variant="secondary" className="shrink-0">
                    {keyword.occurrences.length}
                  </Badge>
                </div>
                <div className="space-y-1.5">
                  {keyword.occurrences.map((occurrence, index) => (
                    <Button
                      key={index}
                      variant="ghost"
                      size="sm"
                      className="w-full justify-start h-auto py-2 px-3 text-left hover:bg-accent/50"
                      onClick={() => handleKeywordClick(keyword.term, occurrence.timestamp)}
                    >
                      <div className="flex-1 min-w-0">
                        <div className="flex items-center gap-1.5 mb-1">
                          <Clock className="h-3 w-3 text-muted-foreground shrink-0" />
                          <span className="text-xs font-medium text-primary">{occurrence.timestamp}</span>
                        </div>
                        <p className="text-xs text-muted-foreground line-clamp-2 text-pretty">{occurrence.context}</p>
                      </div>
                    </Button>
                  ))}
                </div>
              </div>
            ))}
          </div>
        </ScrollArea>
      </CardContent>
    </Card>
  )
}
