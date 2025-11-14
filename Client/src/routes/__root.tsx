import {
  HeadContent,
  createRootRouteWithContext,
} from "@tanstack/react-router";

import appStyle from "../styles.css?url";
import loaderStyle from "../style/loader.css?url";

import type { QueryClient } from "@tanstack/react-query";

interface RouterContext {
  queryClient: QueryClient;
}

export const Route = createRootRouteWithContext<RouterContext>()({
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
    links: [
      {
        rel: "stylesheet",
        href: appStyle,
      },
      {
        rel: "stylesheet",
        href: loaderStyle,
      },
    ],
  }),
  shellComponent: RootDocument,
});

function RootDocument({ children }: { children: React.ReactNode }) {
  return (
    <div>
      <HeadContent />
      {children}
    </div>
  );
}
