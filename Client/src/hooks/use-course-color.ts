import { CourseDetailsDto, CourseListDto } from "@/api/web-api-client";
import { useMemo } from "react";
import useTheme from "./use-theme";
import { Color } from "@/lib/color";

export default function useCourseColor(
  course?: CourseListDto | CourseDetailsDto
) {
  const theme = useTheme();

  return useMemo(() => {
    return course?.colour.code ? Color.fromHex(course.colour.code) : theme.primary;
  }, [course]);
}
