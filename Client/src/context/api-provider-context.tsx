import { CoursesClient, LecturesClient, UsersClient } from "@/api/web-api-client";
import axios, { AxiosInstance } from "axios";
import { createContext, useState } from "react";

interface ApiProviderContext {
  axiosInstance: AxiosInstance;
  coursesClient: CoursesClient;
  usersClient: UsersClient;
  lecturesClient: LecturesClient;
}

const defaultApiProviderContext: ApiProviderContext = {
  coursesClient: new CoursesClient(),
  usersClient: new UsersClient(),
  lecturesClient: new LecturesClient(),
  axiosInstance: axios.create(),
};

export const ApiProviderContext = createContext<ApiProviderContext>(
  defaultApiProviderContext
);

export const ApiProvider = ({ children, axiosInstance }: { children: React.ReactNode, axiosInstance: AxiosInstance }) => {

  console.log("Creating API Provider with Axios Instance:", axiosInstance);
  const [context, _] = useState<ApiProviderContext>({
    coursesClient: new CoursesClient(undefined, axiosInstance),
    lecturesClient: new LecturesClient(undefined, axiosInstance),
    usersClient: new UsersClient(undefined, axiosInstance),
    axiosInstance,
  });

  return (
    <ApiProviderContext.Provider value={context}>
      {children}
    </ApiProviderContext.Provider>
  );
};
