import type { Lecture } from "@/lib/types"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Clock, Calendar, FileText, Sparkles } from "lucide-react"
import { Link } from "@tanstack/react-router"

interface LectureCardProps {
  lecture: Lecture
}

export function LectureCard({ lecture }: LectureCardProps) {
  return (
    <Link to="/lecture/$id" params={{ id: lecture.id }}>
      <Card className="hover:border-primary transition-colors cursor-pointer h-full">
        <CardHeader>
          <div className="flex items-start justify-between gap-4">
            <div className="flex-1 min-w-0">
              <CardTitle className="text-lg mb-2 text-balance">{lecture.name}</CardTitle>
              <CardDescription className="flex flex-wrap items-center gap-3 text-sm">
                <span className="flex items-center gap-1.5">
                  <Calendar className="h-3.5 w-3.5" />
                  {new Date(lecture.date).toLocaleDateString("en-US", {
                    month: "short",
                    day: "numeric",
                    year: "numeric",
                  })}
                </span>
                <span className="flex items-center gap-1.5">
                  <Clock className="h-3.5 w-3.5" />
                  {lecture.duration}
                </span>
              </CardDescription>
            </div>
            {lecture.mediaType && (
              <Badge variant="secondary" className="shrink-0">
                {lecture.mediaType}
              </Badge>
            )}
          </div>
        </CardHeader>
        <CardContent>
          <p className="text-sm text-muted-foreground line-clamp-3 mb-4 text-pretty">{lecture.summary}</p>
          <div className="flex items-center gap-4 flex-wrap">
            <div className="flex items-center gap-2">
              <FileText className="h-4 w-4 text-muted-foreground" />
              <span className="text-xs text-muted-foreground">{lecture.keywords.length} keywords</span>
            </div>
            <div className="flex items-center gap-2">
              <Sparkles className="h-4 w-4 text-muted-foreground" />
              <span className="text-xs text-muted-foreground">{lecture.flashcards.length} flashcards</span>
            </div>
          </div>
        </CardContent>
      </Card>
    </Link>
  )
}
