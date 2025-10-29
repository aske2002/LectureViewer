import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { Input } from "@/components/ui/input";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import useAuth from "@/hooks/use-auth-client";
import useLoader from "@/hooks/use-loader";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { LoginRequest } from "@/api/web-api-client";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { LoadingButton } from "@/components/shared/loading-button";
import LinkButton from "@/components/shared/link-button";
import { zodValidator } from "@tanstack/zod-adapter";
import { redirectSearchsearch } from "@/lib/redirectSearch";
import { toast } from "sonner";
import { useEffect } from "react";

export const Route = createFileRoute("/auth/sign-in")({
  component: RouteComponent,
  validateSearch: zodValidator(redirectSearchsearch),
});

const signInSchema = z.object({
  email: z.string().email("Ugyldig email"),
  password: z.string().min(8, {
    message: "Koden er mindst 8 tegn",
  }),
});

function RouteComponent() {
  const { signIn } = useAuth();
  const { isLoading, withLoader } = useLoader();
  const { redirect } = Route.useSearch();
  const { user } = useAuth();

  useEffect(() => {
    if (user) {
      navigate({
        to: redirect,
      });
    }
  }, [user]);

  const navigate = useNavigate();

  const form = useForm<z.infer<typeof signInSchema>>({
    resolver: zodResolver(signInSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });

  const onSubmit = async (data: z.infer<typeof signInSchema>) => {
    await withLoader(
      signIn.mutateAsync(
        new LoginRequest({
          email: data.email,
          password: data.password,
          twoFactorCode: undefined,
          twoFactorRecoveryCode: undefined,
        })
      )
    );
    toast.success("Logget ind", {
      richColors: true,
    });
    navigate({
      to: redirect,
    });
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle className="text-2xl">Log ind</CardTitle>
        <CardDescription>
          Log ind for at få adgang til din konto
        </CardDescription>
      </CardHeader>
      <CardContent>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
            <div className="flex flex-col gap-6">
              <div className="grid gap-2">
                <FormField
                  control={form.control}
                  name="email"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Email</FormLabel>
                      <FormControl>
                        <Input placeholder="nb@example.com" {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>
              <div className="grid gap-2">
                <FormField
                  control={form.control}
                  name="password"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Password</FormLabel>
                      <FormControl>
                        <Input
                          type="password"
                          placeholder="Adgangskode"
                          {...field}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>
              <LoadingButton
                type="submit"
                className="w-full"
                loading={isLoading}
              >
                Log ind
              </LoadingButton>
            </div>
            <div className="mt-4 text-center text-sm">
              Har du ikke en konto?{" "}
              <LinkButton
                className="p-0"
                variant={"link"}
                to="/auth/sign-up"
                search={(prev) => prev}
              >
                Opret konto
              </LinkButton>
            </div>
          </form>
        </Form>
      </CardContent>
    </Card>
  );
}
