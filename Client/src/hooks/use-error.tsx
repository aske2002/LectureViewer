import {
  HttpValidationProblemDetails,
  ProblemDetails,
} from "@/api/web-api-client";

type ErrorResponse = {
  title: string;
  description: string;
  error: Error | ProblemDetails;
};

export default function useError(
  defaultMessage: {
    title: string;
    description: string;
  } = { title: "Fejl", description: "En ukendt fejl opstod, prøv igen senere" }
) {
  const getFromProblemDetails = (
    error: ProblemDetails
  ): { title: string; description: string } => {
    console.log(error);

    if (error instanceof HttpValidationProblemDetails) {
      return {
        title: defaultMessage.title,
        description:
          Object.values(error.errors || {})
            .at(0)
            ?.at(0) || defaultMessage.description,
      };
    }
    return {
      title: defaultMessage.title,
      description: error.detail || defaultMessage.description,
    };
  };

  function getMessage(error: unknown): ErrorResponse {
    if (error instanceof Error) {
      return {
        title: defaultMessage.title,
        description: error.message,
        error,
      };
    } else if (error instanceof ProblemDetails) {
      return { ...getFromProblemDetails(error), error };
    } else {
      return {
        title: defaultMessage.title,
        description: defaultMessage.description,
        error: new Error(defaultMessage.description),
      };
    }
  }

  return {
    getMessage,
    getFromProblemDetails,
  };
}
