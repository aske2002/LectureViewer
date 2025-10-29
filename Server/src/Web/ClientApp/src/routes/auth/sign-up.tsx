import { createFileRoute, Navigate, useNavigate } from "@tanstack/react-router";
import useAuth from "@/hooks/use-auth-client";
import useLoader from "@/hooks/use-loader";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { LoadingButton } from "@/components/shared/loading-button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { CreateUserCommand, LoginRequest } from "@/app/web-api-client";
import LinkButton from "@/components/shared/link-button";
import { redirectSearchsearch } from "@/lib/redirectSearch";
import { zodValidator } from "@tanstack/zod-adapter";
import { toast } from "sonner";
import { useEffect } from "react";

export const Route = createFileRoute("/auth/sign-up")({
  component: RouteComponent,
  validateSearch: zodValidator(redirectSearchsearch),
});

const signupSchema = z
  .object({
    email: z.string().email("Ugyldig email"),
    password: z.string().min(8, {
      message: "Kode skal være mindst 8 tegn",
    }),
    firstName: z.string().min(1, {
      message: "Fornavn skal være mindst 1 tegn",
    }),
    lastName: z.string().min(1, {
      message: "Efternavn skal være mindst 1 tegn",
    }),
    confirmPassword: z.string(),
  })
  .superRefine((data, ctx) => {
    if (data.password !== data.confirmPassword) {
      ctx.addIssue({
        path: ["confirmPassword"],
        message: "Kodeordene skal være ens",
        code: z.ZodIssueCode.custom,
      });
    }
  });

function RouteComponent() {
  const { register, signIn } = useAuth();
  const navigate = useNavigate();
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

  const form = useForm<z.infer<typeof signupSchema>>({
    resolver: zodResolver(signupSchema),
    defaultValues: {
      email: "",
      password: "",
      confirmPassword: "",
      lastName: "",
      firstName: "",
    },
  });

  const onSubmit = async (data: z.infer<typeof signupSchema>) => {
    await withLoader(
      register
        .mutateAsync(
          new CreateUserCommand({
            email: data.email,
            password: data.password,
            firstName: data.firstName,
            lastName: data.lastName,
          })
        )
        .then(() =>
          signIn.mutateAsync(
            new LoginRequest({
              email: data.email,
              password: data.password,
              twoFactorCode: undefined,
              twoFactorRecoveryCode: undefined,
            })
          )
        )
    );
    toast.success("Konto oprettet", {
      richColors: true,
    });
    navigate({
      to: redirect,
    });
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle className="text-2xl">Opret</CardTitle>
        <CardDescription>
          Indtast din information for at oprette en konto
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
              <div className="flex gap-2">
                <FormField
                  control={form.control}
                  name="firstName"
                  render={({ field }) => (
                    <FormItem className="flex-1">
                      <FormLabel>Fornavn</FormLabel>
                      <FormControl>
                        <Input placeholder="Fornavn" {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="lastName"
                  render={({ field }) => (
                    <FormItem className="flex-1">
                      <FormLabel>Efternavn</FormLabel>
                      <FormControl>
                        <Input placeholder="Efternavn" {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>
              <div className="grid gap-2">
                <div className="flex items-stretch center flex-col gap-2">
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
                  <FormField
                    control={form.control}
                    name="confirmPassword"
                    render={({ field }) => (
                      <FormItem>
                        <FormControl>
                          <Input
                            type="password"
                            placeholder="Gentag adgangskode"
                            {...field}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                </div>
              </div>
              <LoadingButton
                type="submit"
                className="w-full"
                loading={isLoading}
              >
                Opret konto
              </LoadingButton>
            </div>
            <div className="mt-4 text-center text-sm">
              Har du allerede en konto?{" "}
              <LinkButton
                className="p-0"
                variant={"link"}
                to="/auth/sign-in"
                search={(prev) => prev}
              >
                Log ind
              </LinkButton>
            </div>
          </form>
        </Form>
      </CardContent>
    </Card>
  );
}
