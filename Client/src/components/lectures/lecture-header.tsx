import type { Lecture } from "@/lib/types";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import {
  ArrowLeft,
  Calendar,
  Clock,
  Download,
  Sparkles,
  Hash,
} from "lucide-react";
import { Link } from "@tanstack/react-router";
import {
  CoursePermissionType,
  LectureDto,
  PolicyDto,
} from "@/api/web-api-client";
import { useMemo } from "react";
import dayjs from "dayjs";
import RequireAuthorization from "../shared/require-authorization";
import { useCourseApi } from "@/api/use-course-api";
import { UploadLectureContentDialog } from "./upload-lecture-content-dialog";

interface LectureHeaderProps {
  lecture: LectureDto;
  courseId: string;
}

export function LectureHeader({ lecture, courseId }: LectureHeaderProps) {
  const { duration } = useMemo(() => {
    const durationMinutes = dayjs(lecture.endDate).diff(
      dayjs(lecture.startDate),
      "minute"
    );
    const hours = Math.floor(durationMinutes / 60);
    const minutes = durationMinutes % 60;
    return {
      duration: `${hours > 0 ? `${hours}h ` : ""}${minutes}m`,
    };
  }, [lecture.endDate, lecture.startDate]);

  const {
    coursePermissions: { data: coursePermissions },
    course: { data: course },
  } = useCourseApi(courseId);

  return (
    <header className="border-b border-border bg-card">
      <div className="container mx-auto px-4 py-6">
        <div className="flex items-start justify-between gap-4 mb-4">
          <Link to="/course/$courseId" params={{ courseId: courseId }}>
            <Button variant="ghost" size="sm" className="gap-2">
              <ArrowLeft className="h-4 w-4" />
              Back to Lectures
            </Button>
          </Link>
          <div className="flex items-center gap-2">
            {coursePermissions?.permissions.includes(
              CoursePermissionType.UploadCourseContent
            ) &&
              course && <UploadLectureContentDialog course={course} lecture={lecture}/>}
            <Button
              variant="outline"
              size="sm"
              className="gap-2 bg-transparent"
            >
              <Download className="h-4 w-4" />
              Export
            </Button>
          </div>
        </div>

        <div className="space-y-4">
          <div className="flex items-start justify-between gap-4">
            <h1 className="text-3xl font-bold text-balance">{lecture.title}</h1>
            {/* {lecture.mediaType && (
              <Badge variant="secondary" className="shrink-0">
                {lecture.mediaType}
              </Badge>
            )} */}
          </div>

          <div className="flex flex-wrap items-center gap-4 text-sm text-muted-foreground">
            <span className="flex items-center gap-1.5">
              <Calendar className="h-4 w-4" />
              {new Date(lecture.startDate).toLocaleDateString("en-US", {
                month: "long",
                day: "numeric",
                year: "numeric",
              })}
            </span>
            <span className="flex items-center gap-1.5">
              <Clock className="h-4 w-4" />
              {duration}
            </span>
            {/* <span className="flex items-center gap-1.5">
              <Hash className="h-4 w-4" />
              {lecture.keywords.length} keywords
            </span>
            <span className="flex items-center gap-1.5">
              <Sparkles className="h-4 w-4" />
              {lecture.flashcards.length} flashcards
            </span> */}
          </div>

          {/* <p className="text-muted-foreground text-pretty max-w-3xl">
            {lecture.summary}
          </p> */}
        </div>
      </div>
    </header>
  );
}
