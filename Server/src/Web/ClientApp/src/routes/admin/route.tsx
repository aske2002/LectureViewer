import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { Link, LinkOptions, Outlet } from "@tanstack/react-router";
import { useEffect, useState } from "react";
import {
  CreditCard,
  Globe,
  LayoutDashboard,
  Loader2,
  LogOut,
  Menu,
  Plane,
  Settings,
  User,
} from "lucide-react";

import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Sheet, SheetContent, SheetTrigger } from "@/components/ui/sheet";
import { cn } from "@/lib/utils";
import useLoader from "@/hooks/use-loader";
import { authLoader } from "@/lib/authenticatedRoute";
import redirectToLogin from "@/lib/redirectToLogin";
import useAuth, { AuthState } from "@/hooks/use-auth-client";

export const Route = createFileRoute("/admin")({
  loader: authLoader,
  component: RouteComponent,
});

type ValidPath = LinkOptions["to"];

interface NavItem {
  link: ValidPath;
  title: string;
  icon: React.ComponentType<{ className?: string }>;
}

const navItems = [
  {
    title: "Dashboard",
    link: "/admin",
    icon: LayoutDashboard,
  },
  {
    title: "Trips",
    link: "/admin/trips",
    icon: Globe,
  },
  {
    title: "Destinations",
    link: "/admin/destinations",
    icon: Plane,
  },
  {
    title: "Donations",
    link: "/admin/donations",
    icon: CreditCard,
  },
  {
    title: "Countries",
    link: "/admin/countries",
    icon: Globe,
  }
] as const satisfies NavItem[];

function RouteComponent() {
  const navigate = useNavigate();
  const { authState, signOut } = useAuth();
  const { user } = Route.useLoaderData();

  useEffect(() => {
    console.log("authState", authState);
    if (authState === AuthState.SignedOut) {
      navigate(redirectToLogin());
    }
  }, [authState]);

  const { isLoading, withLoader } = useLoader();
  const [open, setOpen] = useState(false);

  return (
    <div className="flex min-h-screen flex-col">
      <header className="sticky top-0 z-30 flex h-16 items-center gap-4 border-b bg-background px-4 md:px-6">
        <Sheet open={open} onOpenChange={setOpen}>
          <SheetTrigger asChild>
            <Button variant="outline" size="icon" className="md:hidden">
              <Menu className="h-5 w-5" />
              <span className="sr-only">Toggle Menu</span>
            </Button>
          </SheetTrigger>
          <SheetContent side="left" className="w-72">
            <nav className="grid gap-2 text-lg font-medium">
              <Link
                to="/"
                className="flex items-center gap-2 text-lg font-semibold"
                onClick={() => setOpen(false)}
              >
                <Globe className="h-6 w-6" />
                <span>GSEA Admin</span>
              </Link>
              {navItems.map((item) => (
                <Link
                  key={item.link}
                  to={item.link}
                  activeOptions={{
                    exact: true,
                  }}
                  activeProps={{
                    className:
                      "flex items-center gap-2 rounded-lg px-3 py-2 text-muted-foreground hover:text-foreground bg-amber-500",
                  }}
                  className={
                    "flex items-center gap-2 rounded-lg px-3 py-2 text-muted-foreground hover:text-foreground bg-amber-500"
                  }
                >
                  <item.icon className="h-5 w-5" />
                  {item.title}
                </Link>
              ))}
            </nav>
          </SheetContent>
        </Sheet>
        <Link to="/" className="flex items-center gap-2 text-lg font-semibold">
          <Globe className="h-6 w-6" />
          <span className="hidden md:inline-block">GSEA Admin</span>
        </Link>
        <div className="flex-1"></div>
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="outline" size="icon" className="rounded-full">
              <User className="h-5 w-5" />
              <span className="sr-only">Toggle user menu</span>
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            <DropdownMenuLabel>
              {user?.info?.firstName} {user.info?.lastName}
            </DropdownMenuLabel>
            <DropdownMenuSeparator />
            <DropdownMenuItem>
              <User className="mr-2 h-4 w-4" />
              Profile
            </DropdownMenuItem>
            <DropdownMenuItem>
              <Settings className="mr-2 h-4 w-4" />
              Settings
            </DropdownMenuItem>
            <DropdownMenuSeparator />
            <DropdownMenuItem
              onClick={(e) => {
                e.preventDefault();
                withLoader(signOut);
              }}
              disabled={isLoading}
            >
              {isLoading && <Loader2 className="ml-2 h-4 w-4 animate-spin" />}
              {!isLoading && (
                <>
                  <LogOut className="mr-2 h-4 w-4" />
                  Log out
                </>
              )}
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </header>
      <div className="grid flex-1 md:grid-cols-[220px_1fr]">
        <nav className="hidden border-r bg-muted/40 md:block">
          <div className="grid gap-2 p-4 text-sm">
            {navItems.map((item) => (
              <Link
                key={item.link}
                to={item.link}
                activeOptions={{
                  exact: true,
                }}
                activeProps={{ className: "bg-muted text-foreground" }}
                className={cn(
                  "flex items-center gap-2 rounded-lg px-3 py-2 text-muted-foreground hover:text-foreground"
                )}
              >
                <item.icon className="h-4 w-4" />
                {item.title}
              </Link>
            ))}
          </div>
        </nav>
        <main className="flex-1">{<Outlet />}</main>
      </div>
    </div>
  );
}
