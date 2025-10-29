import {
  LoginRequest,
  CreateUserCommand,
  UsersClient,
} from "@/app/web-api-client";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import axios from "axios";
import { useMemo } from "react";

export enum AuthState {
  SignedIn,
  SignedOut,
  Loading,
}

export const userClient = new UsersClient();
export const userInfoQueryKey = ["user"] as const;

export function signalToCancelToken(signal: AbortSignal) {
  return new axios.CancelToken((cancel) => {
    signal.addEventListener("abort", () => cancel());
  });
}

export const userInfoQuery = {
  queryKey: userInfoQueryKey,
  queryFn: ({ signal }: { signal: AbortSignal }) =>
    userClient.getInfo(signalToCancelToken(signal)).catch(() => null),
};

export default function useAuth() {
  const userQuery = useQuery(userInfoQuery);
  const queryClient = useQueryClient();

  const { data: user, isLoading: isLoadingUser, refetch } = userQuery;

  const signIn = useMutation({
    mutationFn: (request: LoginRequest) =>
      userClient.postApiUsersLogin(true, true, request),
    onSuccess: () => queryClient.invalidateQueries(userInfoQuery),
  });

  const register = useMutation({
    mutationFn: (request: CreateUserCommand) =>
      userClient.register(new CreateUserCommand(request)),
  });

  const signOut = async () => {
    await userClient.signOut();
    await refetch();
  };

  const authState = useMemo(() => {
    if (isLoadingUser || signIn.isPending) {
      return AuthState.Loading;
    }
    if (user) {
      return AuthState.SignedIn;
    }
    return AuthState.SignedOut;
  }, [user, isLoadingUser, signIn.isPending]);

  return {
    client: userClient,
    authState,
    user,
    isLoading: isLoadingUser,
    signIn,
    register,
    signOut,
  };
}
