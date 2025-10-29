import LinkButton from "@/components/shared/link-button";
import { createFileRoute, Outlet } from "@tanstack/react-router";
import { ArrowLeft } from "lucide-react";

export const Route = createFileRoute("/auth")({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <div className="flex min-h-svh w-full items-center justify-center p-6 md:p-10">
      <div className="w-full max-w-sm">
        <div className={"flex flex-col gap-6"}>
          <Outlet />
          <div className="flex w-full justify-center">
            <LinkButton variant={"link"} to={"/"}>
              <ArrowLeft />
              Tilbage til forsiden
            </LinkButton>
          </div>
        </div>
      </div>
    </div>
  );
}
