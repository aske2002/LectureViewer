import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { BookOpen, Clock, User } from "lucide-react"
import { Link } from "@tanstack/react-router"
import { CourseListDto, Season } from "@/api/web-api-client"

interface CourseCardProps {
  course: CourseListDto
}

export function CourseCard({ course }: CourseCardProps) {
  return (
    <Link to="/course/$courseId" params={{ courseId: course.id.toString() }}>
      <Card className="hover:shadow-lg transition-shadow cursor-pointer h-full">
        <CardHeader>
          <div className="flex items-start justify-between mb-2">
            {/* TODO: Implement course color */}
            <div className={`h-12 w-12 rounded-lg flex items-center justify-center`}> 
              <BookOpen className="h-6 w-6 text-white" />
            </div>
            <Badge variant="secondary">{course.internalIdentifier}</Badge>
          </div>
          <CardTitle className="text-xl text-balance">{course.name}</CardTitle>
          <CardDescription className="text-pretty">{course.description}</CardDescription>
        </CardHeader>
        <CardContent>
          <div className="space-y-2 text-sm text-muted-foreground">
            <div className="flex items-center gap-2">
              <User className="h-4 w-4" />
              <span>{course.instructors.map(i => `${i.firstName} ${i.lastName}`).join(", ")}</span>
            </div>
            <div className="flex items-center gap-2">
              <BookOpen className="h-4 w-4" />
              <span>
                {course.lecturesCount} lecture{course.lecturesCount !== 1 ? "s" : ""}
              </span>
            </div>
            <div className="flex items-center gap-2">
              <Clock className="h-4 w-4" />
              <span>{"ToBeCreated"} total</span>
            </div>
          </div>
          <div className="mt-4 pt-4 border-t border-border">
            <Badge variant="outline">{Season[course.semester.season]} {course.semester.year}</Badge>
          </div>
        </CardContent>
      </Card>
    </Link>
  )
}
