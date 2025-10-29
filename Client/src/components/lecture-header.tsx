import type { Lecture } from "@/lib/types"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { ArrowLeft, Calendar, Clock, Download, Sparkles, Hash } from "lucide-react"
import { Link } from "@tanstack/react-router"

interface LectureHeaderProps {
  lecture: Lecture
}

export function LectureHeader({ lecture }: LectureHeaderProps) {
  return (
    <header className="border-b border-border bg-card">
      <div className="container mx-auto px-4 py-6">
        <div className="flex items-start justify-between gap-4 mb-4">
          <Link to="/">
            <Button variant="ghost" size="sm" className="gap-2">
              <ArrowLeft className="h-4 w-4" />
              Back to Lectures
            </Button>
          </Link>
          <Button variant="outline" size="sm" className="gap-2 bg-transparent">
            <Download className="h-4 w-4" />
            Export
          </Button>
        </div>

        <div className="space-y-4">
          <div className="flex items-start justify-between gap-4">
            <h1 className="text-3xl font-bold text-balance">{lecture.name}</h1>
            {lecture.mediaType && (
              <Badge variant="secondary" className="shrink-0">
                {lecture.mediaType}
              </Badge>
            )}
          </div>

          <div className="flex flex-wrap items-center gap-4 text-sm text-muted-foreground">
            <span className="flex items-center gap-1.5">
              <Calendar className="h-4 w-4" />
              {new Date(lecture.date).toLocaleDateString("en-US", {
                month: "long",
                day: "numeric",
                year: "numeric",
              })}
            </span>
            <span className="flex items-center gap-1.5">
              <Clock className="h-4 w-4" />
              {lecture.duration}
            </span>
            <span className="flex items-center gap-1.5">
              <Hash className="h-4 w-4" />
              {lecture.keywords.length} keywords
            </span>
            <span className="flex items-center gap-1.5">
              <Sparkles className="h-4 w-4" />
              {lecture.flashcards.length} flashcards
            </span>
          </div>

          <p className="text-muted-foreground text-pretty max-w-3xl">{lecture.summary}</p>
        </div>
      </div>
    </header>
  )
}
