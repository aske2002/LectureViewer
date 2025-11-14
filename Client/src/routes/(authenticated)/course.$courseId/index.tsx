import { createFileRoute, Link } from "@tanstack/react-router";
import { LectureCard } from "@/components/lectures/lecture-card";
import { Button } from "@/components/ui/button";
import { ArrowLeft, BookOpen, Clock, User } from "lucide-react";
import { Badge } from "@/components/ui/badge";
import RequireAuthorization from "@/components/shared/require-authorization";
import { CoursePermissionType, RoleDto, Season } from "@/api/web-api-client";
import { useCourseApi } from "@/api/use-course-api";
import { FullScreenLoader } from "@/components/shared/loader";
import useCourseColor from "@/hooks/use-course-color";
import { CreateLectureDialog } from "@/components/lectures/create-lecture-dialog";

export const Route = createFileRoute("/(authenticated)/course/$courseId/")({
  component: RouteComponent,
});

function RouteComponent() {
  const { courseId } = Route.useParams();

  const {
    course: { data: course, isLoading },
    coursePermissions: { data: coursePermissions },
  } = useCourseApi(courseId);

  const courseColor = useCourseColor(course);

  if (isLoading || !course) {
    return <FullScreenLoader descriptionText="Loading course" />;
  }

  return (
    <>
      <header className="border-b border-border bg-card">
        <div className="container mx-auto px-4 py-6">
          <Link to="/">
            <Button variant="ghost" size="sm" className="mb-4">
              <ArrowLeft className="h-4 w-4 mr-2" />
              Back to Courses
            </Button>
          </Link>

          <div className="flex items-start justify-between mb-6">
            <div className="flex items-start gap-4">
              <div
                style={{ backgroundColor: courseColor.toHex() }}
                className={`h-16 w-16 rounded-lg flex items-center justify-center shrink-0`}
              >
                <BookOpen className="h-8 w-8 text-white" />
              </div>
              <div>
                <div className="flex items-center gap-2 mb-2">
                  <h1 className="text-3xl font-bold text-balance">
                    {course.name}
                  </h1>
                  <Badge variant="secondary">{course.internalIdentifier}</Badge>
                </div>
                <p className="text-muted-foreground mb-3 text-pretty">
                  {course.description}
                </p>
                <div className="flex items-center  gap-4 text-sm text-muted-foreground">
                  <div className="flex items-center gap-2">
                    <User className="h-4 w-4" />
                    <span>
                      {course.instructors
                        .map((i) => `${i.firstName} ${i.lastName}`)
                        .join(", ")}
                    </span>
                  </div>
                  <div className="flex items-center gap-2">
                    <Clock className="h-4 w-4" />
                    <span>{course.lectures.length} total lectures</span>
                  </div>
                  <Badge variant="outline">
                    {Season[course.semester.season]}
                  </Badge>
                </div>
              </div>
            </div>
            {coursePermissions?.permissions.includes(
              CoursePermissionType.CreateLectures
            ) && <CreateLectureDialog course={course} />}
          </div>
        </div>
      </header>

      <main className="container mx-auto px-4 py-8">
        <div className="mb-6">
          <h2 className="text-xl font-semibold mb-2">Lectures</h2>
          <p className="text-sm text-muted-foreground">
            {course.lectures.length} lecture
            {course.lectures.length !== 1 ? "s" : ""} in this course
          </p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {course.lectures.map((lecture) => (
            <LectureCard
              key={lecture.id}
              lecture={lecture}
              courseId={course.id}
            />
          ))}
        </div>
      </main>
    </>
  );
}
