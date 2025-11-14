import { useMutation, UseMutationOptions } from "@tanstack/react-query";
import { AxiosInstance, AxiosProgressEvent } from "axios";
import { useState } from "react";

export function useFileUpload<
  TClient,
  TReturn extends any,
  TParams extends Record<string, any>,
>({
  client,
  axiosInstance,
  mutationFn,
  ...options
}: Omit<UseMutationOptions<TReturn, Error, TParams>, "mutationFn"> & {
  mutationFn: (client: TClient, data: TParams) => Promise<TReturn>;
  client: TClient;
  axiosInstance: AxiosInstance;
}) {
  const [progress, setProgress] = useState(0);

  const mutation = useMutation({
    ...options,
    mutationFn: async (options: TParams) => {
      axiosInstance.defaults.onUploadProgress = (
        event: AxiosProgressEvent
      ) => {
        if (event.lengthComputable) {
          const percentCompleted = event.total
            ? Math.round((event.loaded * 100) / event.total)
            : 0;
          setProgress(percentCompleted);
        }
      };

      return await mutationFn(client, options).finally(() => {
        axiosInstance.defaults.onUploadProgress = undefined;
      });
    },
  });

  return { ...mutation, progress };
}
