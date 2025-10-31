import { useCoursesApi } from "@/api/use-courses-api";
import { PolicyDto } from "@/api/web-api-client";
import { CourseCard } from "@/components/course-card";
import { CreateCourseDialog } from "@/components/create-course-dialog";
import RequireAuthorization from "@/components/shared/require-authorization";
import { Button } from "@/components/ui/button";
import { mockCourses } from "@/lib/mock-data";
import { createFileRoute } from "@tanstack/react-router";
import { BookOpen, Plus } from "lucide-react";

export const Route = createFileRoute("/(authenticated)/")({
  component: RouteComponent,
});

function RouteComponent() {
  const {
    courses: { isLoading, data },
  } = useCoursesApi();
  console.log(data);

  return (
    <>
      <header className="border-b border-border bg-card">
        <div className="container mx-auto px-4 py-6">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-3">
              <div className="h-10 w-10 rounded-lg bg-primary flex items-center justify-center">
                <BookOpen className="h-6 w-6 text-primary-foreground" />
              </div>
              <div>
                <h1 className="text-2xl font-bold text-balance">My Courses</h1>
                <p className="text-sm text-muted-foreground">
                  Manage your courses and lectures
                </p>
              </div>
            </div>
            <RequireAuthorization allowPolicies={[PolicyDto.CanCreateCourses]}>
              <CreateCourseDialog />
            </RequireAuthorization>
          </div>
        </div>
      </header>

      <main className="container mx-auto px-4 py-8">
        <div className="mb-6">
          <h2 className="text-xl font-semibold mb-2">All Courses</h2>
          <p className="text-sm text-muted-foreground">
            {mockCourses.length} course{mockCourses.length !== 1 ? "s" : ""} for
            Spring 2025
          </p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {data?.items.map((course) => (
            <CourseCard key={course.id} course={course} />
          ))}
        </div>
      </main>
    </>
  );
}
