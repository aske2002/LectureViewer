import { redirect } from "@tanstack/react-router";

export default function redirectToLogin() {
  return redirect({
    to: "/auth/sign-in",
    search: { redirect: location.pathname },
  });
}
