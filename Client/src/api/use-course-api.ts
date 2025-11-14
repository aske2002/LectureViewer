import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import useApi from "./use-api";
import { CreateLectureCommand, ICreateLectureCommand } from "./web-api-client";

export function useCourseApi(courseId: string) {
  const { coursesClient } = useApi();

  const queryClient = useQueryClient();

  const course = useQuery({
    queryKey: ["course-details", courseId],
    queryFn: async () => {
      return coursesClient.getCourseById(courseId);
    },
  });

  const coursePermissions = useQuery({
    queryKey: ["course-permissions", courseId],
    queryFn: async () => {
      return coursesClient.getCoursePermissions(courseId);
    },
  });

  const createLecture = useMutation({
    mutationFn: async (data: { id: string; body: ICreateLectureCommand }) => {
      return coursesClient.addLectureToCourse(
        data.id,
        new CreateLectureCommand(data.body)
      );
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["course-details", courseId] });
    },
  });

  return {
    course,
    coursePermissions,
    createLecture,
  };
}
