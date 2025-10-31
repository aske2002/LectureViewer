import { CoursesClient } from "./web-api-client";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import useApi from "./use-api";

export function useCoursesApi() {
  const { coursesClient } = useApi();

  const queryClient = useQueryClient();

  const courses = useQuery({
    queryKey: ["list-courses"],
    queryFn: async () => {
      return coursesClient.listCourses();
    },
  });

  const createCourse = useMutation({
    mutationKey: ["create-course"],
    mutationFn: async (data: Parameters<CoursesClient["createCourse"]>[0]) => {
      return coursesClient.createCourse(data);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["list-courses"] });
    }
  });

  return {
    createCourse,
    courses,
  };
}
