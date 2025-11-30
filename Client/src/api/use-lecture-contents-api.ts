import { useQuery, useQueryClient } from "@tanstack/react-query";
import useApi from "./use-api";


export function useLectureContentsApi(courseId: string, lectureId: string) {
  const { lecturesClient, axiosInstance } = useApi();

  const queryClient = useQueryClient();

  const lectureContents = useQuery({
    queryKey: ["lecture-contents", courseId, lectureId],
    queryFn: async () => {
      return lecturesClient.listLectureContents(courseId, lectureId);
    },
  });

  const invalidateLectureContents = async () => {
    return await queryClient.invalidateQueries({
      queryKey: ["lecture-contents", courseId, lectureId],
    });
  }

  return {
    lectureContents,
    lecturesClient,
    axiosInstance,
    invalidateLectureContents,
  };
}
