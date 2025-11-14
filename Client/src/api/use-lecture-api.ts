import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import useApi from "./use-api";
import { useFileUpload } from "./use-file-upload";

export function useLectureApi(courseId: string, lectureId: string) {
  const { lecturesClient, axiosInstance } = useApi();

  const queryClient = useQueryClient();

  const lecture = useQuery({
    queryKey: ["lecture-details", courseId, lectureId],
    queryFn: async () => {
      return lecturesClient.getLectureDetails(courseId, lectureId);
    },
  });

  const invalidateLecture = async () => {
    return await queryClient.invalidateQueries({
      queryKey: ["lecture-details", courseId, lectureId],
    });
  }

  return {
    lecture,
    lecturesClient,
    axiosInstance,
    invalidateLecture,
  };
}
