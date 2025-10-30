import { CoursesClient, UsersClient } from "@/api/web-api-client";
import { createContext, useState } from "react";

interface ApiProviderContext {
  coursesClient: CoursesClient;
  usersClient: UsersClient;
}

const defaultApiProviderContext: ApiProviderContext = {
  coursesClient: new CoursesClient(),
  usersClient: new UsersClient(),
};

export const ApiProviderContext = createContext<ApiProviderContext>(
  defaultApiProviderContext
);

export const ApiProvider = ({ children }: { children: React.ReactNode }) => {
  const [context, _] = useState<ApiProviderContext>({
    coursesClient: new CoursesClient(),
    usersClient: new UsersClient(),
  });

  return (
    <ApiProviderContext.Provider value={context}>
      {children}
    </ApiProviderContext.Provider>
  );
};
