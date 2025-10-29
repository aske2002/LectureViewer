import ReactDOM from "react-dom/client";
import "./styles/globals.css";

// Import the generated route tree
import { routeTree } from "./routeTree.gen";
import { createRouter, RouterProvider } from "@tanstack/react-router";
import { QueryClient } from "@tanstack/react-query";
import { QueryClientProvider } from "@tanstack/react-query";
import { Toaster } from "./components/ui/sonner";
import DefaultErrorComponent from "./components/shared/default-error-component";

const queryClient = new QueryClient();

// Create a new router instance
const router = createRouter({
  routeTree,
  context: {
    queryClient,
  },
});

// Register the router instance for type safety
declare module "@tanstack/react-router" {
  interface Register {
    router: typeof router;
  }
}

// Render the app
const rootElement = document.getElementById("root")!;
if (!rootElement.innerHTML) {
  const root = ReactDOM.createRoot(rootElement);
  root.render(
    <QueryClientProvider client={queryClient}>
      <InnerApp />
      <Toaster />
    </QueryClientProvider>
  );
}
function InnerApp() {
  return (
    <RouterProvider
      router={router}
      defaultErrorComponent={DefaultErrorComponent}
    />
  );
}
