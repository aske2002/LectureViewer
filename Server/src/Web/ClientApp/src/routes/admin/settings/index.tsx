import { createFileRoute } from "@tanstack/react-router";

import { cn } from "@/lib/utils";

import type React from "react";

import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";

import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Separator } from "@/components/ui/separator";
import { Switch } from "@/components/ui/switch";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Textarea } from "@/components/ui/textarea";

export const Route = createFileRoute("/admin/settings/")({
  component: RouteComponent,
});

const profileFormSchema = z.object({
  organizationName: z.string().min(2, {
    message: "Organization name must be at least 2 characters.",
  }),
  email: z.string().email({
    message: "Please enter a valid email address.",
  }),
  bio: z.string().max(500, {
    message: "Bio must not be longer than 500 characters.",
  }),
  website: z.string().url({
    message: "Please enter a valid URL.",
  }),
});

const notificationsFormSchema = z.object({
  newDonation: z.boolean(),
  newTrip: z.boolean(),
  tripUpdates: z.boolean(),
  marketingEmails: z.boolean(),
});

type ProfileFormValues = z.infer<typeof profileFormSchema>;
type NotificationsFormValues = z.infer<typeof notificationsFormSchema>;

export default function RouteComponent() {
  const profileForm = useForm<ProfileFormValues>({
    resolver: zodResolver(profileFormSchema),
    defaultValues: {
      organizationName: "Global Student Exchange Association",
      email: "contact@gsea.edu",
      bio: "We help university students experience the world through study abroad programs, cultural exchanges, and research trips.",
      website: "https://gsea.edu",
    },
  });

  const notificationsForm = useForm<NotificationsFormValues>({
    resolver: zodResolver(notificationsFormSchema),
    defaultValues: {
      newDonation: true,
      newTrip: true,
      tripUpdates: true,
      marketingEmails: false,
    },
  });

  function onProfileSubmit(data: ProfileFormValues) {
    console.log(data);
  }

  function onNotificationsSubmit(data: NotificationsFormValues) {
    console.log(data);
  }

  return (
    <div className="flex-1 space-y-4 p-4 pt-6 md:p-8">
      <div>
        <h2 className="text-3xl font-bold tracking-tight">Settings</h2>
        <p className="text-muted-foreground">
          Manage your account settings and preferences.
        </p>
      </div>
      <Separator />
      <Tabs defaultValue="general" className="space-y-4">
        <TabsList>
          <TabsTrigger value="general">General</TabsTrigger>
          <TabsTrigger value="notifications">Notifications</TabsTrigger>
          <TabsTrigger value="appearance">Appearance</TabsTrigger>
        </TabsList>
        <TabsContent value="general" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Profile</CardTitle>
              <CardDescription>
                Update your organization information.
              </CardDescription>
            </CardHeader>
            <CardContent>
              <Form {...profileForm}>
                <form
                  onSubmit={profileForm.handleSubmit(onProfileSubmit)}
                  className="space-y-8"
                >
                  <FormField
                    control={profileForm.control}
                    name="organizationName"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Organization Name</FormLabel>
                        <FormControl>
                          <Input
                            placeholder="Your organization name"
                            {...field}
                          />
                        </FormControl>
                        <FormDescription>
                          This is the name that will be displayed on your
                          profile and in emails.
                        </FormDescription>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={profileForm.control}
                    name="email"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Email</FormLabel>
                        <FormControl>
                          <Input placeholder="contact@example.com" {...field} />
                        </FormControl>
                        <FormDescription>
                          This is the email that will be used for notifications
                          and communications.
                        </FormDescription>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={profileForm.control}
                    name="bio"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Bio</FormLabel>
                        <FormControl>
                          <Textarea
                            placeholder="Tell us about your organization"
                            className="resize-none"
                            {...field}
                          />
                        </FormControl>
                        <FormDescription>
                          This will be displayed on your public profile.
                        </FormDescription>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={profileForm.control}
                    name="website"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Website</FormLabel>
                        <FormControl>
                          <Input placeholder="https://example.com" {...field} />
                        </FormControl>
                        <FormDescription>
                          Your organization's website address.
                        </FormDescription>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <Button type="submit">Update profile</Button>
                </form>
              </Form>
            </CardContent>
          </Card>
          <Card>
            <CardHeader>
              <CardTitle>API Keys</CardTitle>
              <CardDescription>
                Manage your API keys for integration with other services.
              </CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="flex items-center justify-between">
                <div>
                  <p className="font-medium">Payment Gateway</p>
                  <p className="text-sm text-muted-foreground">
                    Used for processing donations
                  </p>
                </div>
                <Input
                  className="max-w-[300px] font-mono"
                  value="••••••••••••••••••••••••••••••"
                  disabled
                />
              </div>
              <div className="flex items-center justify-between">
                <div>
                  <p className="font-medium">Email Service</p>
                  <p className="text-sm text-muted-foreground">
                    Used for sending notifications
                  </p>
                </div>
                <Input
                  className="max-w-[300px] font-mono"
                  value="••••••••••••••••••••••••••••••"
                  disabled
                />
              </div>
            </CardContent>
            <CardFooter>
              <Button variant="outline">Regenerate Keys</Button>
            </CardFooter>
          </Card>
        </TabsContent>
        <TabsContent value="notifications" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Notifications</CardTitle>
              <CardDescription>
                Configure how you receive notifications.
              </CardDescription>
            </CardHeader>
            <CardContent>
              <Form {...notificationsForm}>
                <form
                  onSubmit={notificationsForm.handleSubmit(
                    onNotificationsSubmit
                  )}
                  className="space-y-8"
                >
                  <FormField
                    control={notificationsForm.control}
                    name="newDonation"
                    render={({ field }) => (
                      <FormItem className="flex flex-row items-center justify-between rounded-lg border p-4">
                        <div className="space-y-0.5">
                          <FormLabel className="text-base">
                            New Donation Alerts
                          </FormLabel>
                          <FormDescription>
                            Receive notifications when a new donation is made.
                          </FormDescription>
                        </div>
                        <FormControl>
                          <Switch
                            checked={field.value}
                            onCheckedChange={field.onChange}
                          />
                        </FormControl>
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={notificationsForm.control}
                    name="newTrip"
                    render={({ field }) => (
                      <FormItem className="flex flex-row items-center justify-between rounded-lg border p-4">
                        <div className="space-y-0.5">
                          <FormLabel className="text-base">
                            New Trip Alerts
                          </FormLabel>
                          <FormDescription>
                            Receive notifications when a new trip is created.
                          </FormDescription>
                        </div>
                        <FormControl>
                          <Switch
                            checked={field.value}
                            onCheckedChange={field.onChange}
                          />
                        </FormControl>
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={notificationsForm.control}
                    name="tripUpdates"
                    render={({ field }) => (
                      <FormItem className="flex flex-row items-center justify-between rounded-lg border p-4">
                        <div className="space-y-0.5">
                          <FormLabel className="text-base">
                            Trip Updates
                          </FormLabel>
                          <FormDescription>
                            Receive notifications about changes to existing
                            trips.
                          </FormDescription>
                        </div>
                        <FormControl>
                          <Switch
                            checked={field.value}
                            onCheckedChange={field.onChange}
                          />
                        </FormControl>
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={notificationsForm.control}
                    name="marketingEmails"
                    render={({ field }) => (
                      <FormItem className="flex flex-row items-center justify-between rounded-lg border p-4">
                        <div className="space-y-0.5">
                          <FormLabel className="text-base">
                            Marketing Emails
                          </FormLabel>
                          <FormDescription>
                            Receive emails about new features and updates.
                          </FormDescription>
                        </div>
                        <FormControl>
                          <Switch
                            checked={field.value}
                            onCheckedChange={field.onChange}
                          />
                        </FormControl>
                      </FormItem>
                    )}
                  />
                  <Button type="submit">Save preferences</Button>
                </form>
              </Form>
            </CardContent>
          </Card>
        </TabsContent>
        <TabsContent value="appearance" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Appearance</CardTitle>
              <CardDescription>
                Customize how the dashboard looks.
              </CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="space-y-2">
                <Label>Theme</Label>
                <div className="grid grid-cols-3 gap-2">
                  <Button variant="outline" className="justify-start">
                    <SunIcon className="mr-2 h-4 w-4" />
                    Light
                  </Button>
                  <Button variant="outline" className="justify-start">
                    <MoonIcon className="mr-2 h-4 w-4" />
                    Dark
                  </Button>
                  <Button variant="outline" className="justify-start">
                    <LaptopIcon className="mr-2 h-4 w-4" />
                    System
                  </Button>
                </div>
              </div>
              <div className="space-y-2">
                <Label>Accent Color</Label>
                <div className="grid grid-cols-6 gap-2">
                  <Button
                    variant="outline"
                    className="h-8 w-8 rounded-full p-0"
                    style={{ backgroundColor: "hsl(var(--primary))" }}
                  ></Button>
                  <Button
                    variant="outline"
                    className="h-8 w-8 rounded-full p-0 bg-blue-500"
                  ></Button>
                  <Button
                    variant="outline"
                    className="h-8 w-8 rounded-full p-0 bg-green-500"
                  ></Button>
                  <Button
                    variant="outline"
                    className="h-8 w-8 rounded-full p-0 bg-red-500"
                  ></Button>
                  <Button
                    variant="outline"
                    className="h-8 w-8 rounded-full p-0 bg-purple-500"
                  ></Button>
                  <Button
                    variant="outline"
                    className="h-8 w-8 rounded-full p-0 bg-orange-500"
                  ></Button>
                </div>
              </div>
            </CardContent>
            <CardFooter>
              <Button>Save preferences</Button>
            </CardFooter>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}

// Components needed for the settings page
function Label({
  children,
  className,
  ...props
}: React.ComponentProps<"label">) {
  return (
    <label
      className={cn("text-sm font-medium leading-none", className)}
      {...props}
    >
      {children}
    </label>
  );
}

function SunIcon(props: React.ComponentProps<"svg">) {
  return (
    <svg
      {...props}
      xmlns="http://www.w3.org/2000/svg"
      width="24"
      height="24"
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="2"
      strokeLinecap="round"
      strokeLinejoin="round"
    >
      <circle cx="12" cy="12" r="4" />
      <path d="M12 2v2" />
      <path d="M12 20v2" />
      <path d="m4.93 4.93 1.41 1.41" />
      <path d="m17.66 17.66 1.41 1.41" />
      <path d="M2 12h2" />
      <path d="M20 12h2" />
      <path d="m6.34 17.66-1.41 1.41" />
      <path d="m19.07 4.93-1.41 1.41" />
    </svg>
  );
}

function MoonIcon(props: React.ComponentProps<"svg">) {
  return (
    <svg
      {...props}
      xmlns="http://www.w3.org/2000/svg"
      width="24"
      height="24"
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="2"
      strokeLinecap="round"
      strokeLinejoin="round"
    >
      <path d="M12 3a6 6 0 0 0 9 9 9 9 0 1 1-9-9Z" />
    </svg>
  );
}

function LaptopIcon(props: React.ComponentProps<"svg">) {
  return (
    <svg
      {...props}
      xmlns="http://www.w3.org/2000/svg"
      width="24"
      height="24"
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="2"
      strokeLinecap="round"
      strokeLinejoin="round"
    >
      <path d="M20 16V7a2 2 0 0 0-2-2H6a2 2 0 0 0-2 2v9m16 0H4m16 0 1.28 2.55a1 1 0 0 1-.9 1.45H3.62a1 1 0 0 1-.9-1.45L4 16" />
    </svg>
  );
}
