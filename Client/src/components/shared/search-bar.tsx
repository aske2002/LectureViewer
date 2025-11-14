

import { useState } from "react"
import { Input } from "@/components/ui/input"
import { Search, X } from "lucide-react"
import { Button } from "@/components/ui/button"
import { SearchResults } from "@/components/shared/search-results"
import { mockCourses, mockLectures } from "@/lib/mock-data"
import type { SearchResult } from "@/lib/types"

export function SearchBar() {
  const [query, setQuery] = useState("")
  const [results, setResults] = useState<SearchResult[]>([])
  const [isSearching, setIsSearching] = useState(false)

  const handleSearch = (searchQuery: string) => {
    setQuery(searchQuery)

    if (!searchQuery.trim()) {
      setResults([])
      setIsSearching(false)
      return
    }

    setIsSearching(true)

    // Search across all lectures
    const searchResults: SearchResult[] = []
    const lowerQuery = searchQuery.toLowerCase()

    mockLectures.forEach((lecture) => {
      const matches: SearchResult["matches"] = []

      // Search in keywords
      lecture.keywords.forEach((keyword) => {
        if (keyword.term.toLowerCase().includes(lowerQuery)) {
          keyword.occurrences.forEach((occurrence) => {
            matches.push({
              type: "keyword",
              text: keyword.term,
              timestamp: occurrence.timestamp,
              context: occurrence.context,
            })
          })
        }
      })

      // Search in transcript
      const transcriptLines = lecture.transcript.split("\n")
      transcriptLines.forEach((line) => {
        if (line.toLowerCase().includes(lowerQuery)) {
          matches.push({
            type: "transcript",
            text: line.trim(),
            context: line.trim(),
          })
        }
      })

      if (matches.length > 0) {
        searchResults.push({
          lectureId: lecture.id,
          courseId: mockCourses.find((course) =>
            course.lectures.some((l) => l.id === lecture.id)
          )?.id || "",
          lectureName: lecture.name,
          matches: matches.slice(0, 5), // Limit to 5 matches per lecture
        })
      }
    })

    setResults(searchResults)
  }

  const handleClear = () => {
    setQuery("")
    setResults([])
    setIsSearching(false)
  }

  return (
    <div className="relative w-full">
      <div className="relative">
        <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
        <Input
          placeholder="Search across all lectures..."
          className="pl-10 pr-10"
          value={query}
          onChange={(e) => handleSearch(e.target.value)}
        />
        {query && (
          <Button
            variant="ghost"
            size="sm"
            className="absolute right-1 top-1/2 -translate-y-1/2 h-7 w-7 p-0"
            onClick={handleClear}
          >
            <X className="h-4 w-4" />
          </Button>
        )}
      </div>

      {isSearching && <SearchResults results={results} query={query} onClose={handleClear} />}
    </div>
  )
}
