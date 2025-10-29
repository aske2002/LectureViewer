"use client"

import type { SearchResult } from "@/lib/types"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { ScrollArea } from "@/components/ui/scroll-area"
import { FileText, Hash, Clock } from "lucide-react"
import { useEffect, useRef } from "react"
import { Link } from "@tanstack/react-router"

interface SearchResultsProps {
  results: SearchResult[]
  query: string
  onClose: () => void
}

export function SearchResults({ results, query, onClose }: SearchResultsProps) {
  const resultsRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (resultsRef.current && !resultsRef.current.contains(event.target as Node)) {
        onClose()
      }
    }

    document.addEventListener("mousedown", handleClickOutside)
    return () => document.removeEventListener("mousedown", handleClickOutside)
  }, [onClose])

  const highlightQuery = (text: string) => {
    const parts = text.split(new RegExp(`(${query})`, "gi"))
    return parts.map((part, index) =>
      part.toLowerCase() === query.toLowerCase() ? (
        <mark key={index} className="bg-accent text-accent-foreground px-0.5 rounded">
          {part}
        </mark>
      ) : (
        part
      ),
    )
  }

  if (results.length === 0) {
    return (
      <Card ref={resultsRef} className="absolute top-full mt-2 w-full z-50 shadow-lg">
        <CardContent className="py-8 text-center">
          <p className="text-sm text-muted-foreground">No results found for "{query}"</p>
        </CardContent>
      </Card>
    )
  }

  return (
    <Card ref={resultsRef} className="absolute top-full mt-2 w-full z-50 shadow-lg">
      <CardHeader className="pb-3">
        <CardTitle className="text-sm font-medium">
          Found {results.reduce((acc, r) => acc + r.matches.length, 0)} results in {results.length} lecture
          {results.length !== 1 ? "s" : ""}
        </CardTitle>
      </CardHeader>
      <CardContent className="pt-0">
        <ScrollArea className="h-[400px] pr-4">
          <div className="space-y-4">
            {results.map((result) => (
              <div key={result.lectureId} className="space-y-2">
                <Link to="/lecture/$id" params={{ id: result.lectureId.toString() }} onClick={onClose}>
                  <h3 className="font-medium text-sm hover:text-primary transition-colors text-balance">
                    {result.lectureName}
                  </h3>
                </Link>
                <div className="space-y-1.5">
                  {result.matches.map((match, index) => (
                    <Link
                      key={index}
                      to="/lecture/$id"
                      params={{ id: result.lectureId.toString() }}
                      onClick={onClose}
                      className="block p-2 rounded-lg hover:bg-muted transition-colors"
                    >
                      <div className="flex items-start gap-2">
                        {match.type === "keyword" ? (
                          <Hash className="h-4 w-4 text-primary shrink-0 mt-0.5" />
                        ) : (
                          <FileText className="h-4 w-4 text-muted-foreground shrink-0 mt-0.5" />
                        )}
                        <div className="flex-1 min-w-0">
                          {match.timestamp && (
                            <div className="flex items-center gap-1.5 mb-1">
                              <Clock className="h-3 w-3 text-muted-foreground" />
                              <span className="text-xs text-muted-foreground">{match.timestamp}</span>
                              <Badge variant="secondary" className="text-xs">
                                {match.type}
                              </Badge>
                            </div>
                          )}
                          <p className="text-xs text-foreground line-clamp-2 text-pretty">
                            {highlightQuery(match.context)}
                          </p>
                        </div>
                      </div>
                    </Link>
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
