import { UserInfoVm } from "@/app/web-api-client";
import { QueryClient } from "@tanstack/react-query";
import redirectToLogin from "./redirectToLogin";
import { userInfoQuery, userInfoQueryKey } from "@/hooks/use-auth-client";

interface RequiredRouterContext {
  queryClient: QueryClient;
}

export async function authLoader<
  Context extends RequiredRouterContext,
  T extends Record<string, any>,
  Return extends Record<string, any>
>(
  args: { context: Context } & T,
  callback?: (args: T) => Return
): Promise<{ user: UserInfoVm } & Return> {
  const { context } = args;
  const user = await context.queryClient.ensureQueryData(userInfoQuery);
  if (!user) {
    throw redirectToLogin();
  }
  return { ...(callback ? callback(args) : {}), user } as {
    user: UserInfoVm;
  } & Return;
}
