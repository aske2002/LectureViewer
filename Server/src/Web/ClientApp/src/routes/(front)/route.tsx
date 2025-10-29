import Footer from "@/components/layout/footer";
import Header from "@/components/layout/header";
import { createFileRoute, Outlet } from "@tanstack/react-router";

export const Route = createFileRoute("/(front)")({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <div className="flex flex-col min-h-screen">
      <Header />
      <main>
        <Outlet />
      </main>
      <Footer />
    </div>
  );
}
