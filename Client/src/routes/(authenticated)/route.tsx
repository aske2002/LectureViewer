import useAuth, { AuthState } from "@/hooks/use-auth-client";
import { authLoader } from "@/lib/authenticatedRoute";

import {
  createFileRoute,
  HeadContent,
  Outlet,
} from "@tanstack/react-router";
import { useEffect } from "react";

export const Route = createFileRoute("/(authenticated)")({
  loader: authLoader,
  head: () => ({
    meta: [
      {
        charSet: "utf-8",
      },
      {
        name: "viewport",
        content: "width=device-width, initial-scale=1",
      },
      {
        title: "TanStack Start Starter",
      },
    ],
  }),
  component: RouteComponent,
});

function RouteComponent() {
  const navigate = Route.useNavigate();
  const { authState } = useAuth();

  useEffect(() => {
    console.log("authState", authState);
    if (authState === AuthState.SignedOut) {
      navigate({ to: "/auth/sign-in"});
    }
  }, [authState]);


  return (
    <>
      <HeadContent />

      <div className="min-h-screen bg-background">
        <Outlet />
      </div>
    </>
  );
}
