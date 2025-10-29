import { useState } from "react";
import { toast } from "sonner";
import useError from "./use-error";
import { ProblemDetails } from "@/app/web-api-client";

export default function useLoader() {
  const [isLoading, setIsLoading] = useState(false);
  const { getMessage } = useError();

  async function withLoader<T>(
    fn: (() => Promise<T>) | Promise<T>,
    onSuccess?: (data: T) => void,
    onError?: (error: Error | ProblemDetails) => void
  ): Promise<T> {
    setIsLoading(true);
    try {
      setIsLoading(true);
      const result = await (fn instanceof Function ? fn() : fn);
      onSuccess?.(result);
      return result;
    } catch (_e) {
      const error = getMessage(_e);

      if (onError) {
        onError(error.error);
      } else {
        toast.error(error.title, {
          description: error.description,
          richColors: true,
        });
      }

      throw _e;
    } finally {
      setIsLoading(false);
    }
  }

  return {
    isLoading,
    withLoader,
  };
}
