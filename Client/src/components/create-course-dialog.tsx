"use client";

import type React from "react";

import { useState } from "react";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Plus } from "lucide-react";
import {
  CoursesClient,
  CreateCourseCommand,
  ICreateCourseCommand,
  Season,
} from "@/api/web-api-client";
import { useForm } from "react-hook-form";
import z from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "./ui/form";
import { getEnumEntries, getEnumKeys } from "@/lib/getEnumValues";
import { LoadingButton } from "./shared/loading-button";
import { useMutation } from "@tanstack/react-query";
import { useCoursesApi } from "@/api/use-courses-api";

const formDataSchema = z.object({
  name: z.string().min(1, "Course name is required"),
  description: z.string().min(1, "Description is required"),
  internalIdentifier: z.string().min(1, "Course code is required"),
  startDate: z.date(),
  endDate: z.date(),
  semesterSeason: z.nativeEnum(Season),
  semesterYear: z.number().min(2000).max(2100),
});

export function CreateCourseDialog() {
  const [open, setOpen] = useState(false);
  const [formData, setFormData] = useState<ICreateCourseCommand>({
    name: "",
    description: "",
    startDate: new Date(),
    endDate: new Date(),
    internalIdentifier: "",
    semesterSeason: Season.Spring,
    semesterYear: new Date().getFullYear(),
  });

  const { createCourse: {mutateAsync:createCourse, isPending: isCreating}  } = useCoursesApi();

  const form = useForm<z.infer<typeof formDataSchema>>({
    resolver: zodResolver(formDataSchema),
    defaultValues: {
      name: "",
      description: "",
      startDate: new Date(),
      endDate: new Date(),
      internalIdentifier: "",
      semesterSeason: Season.Spring,
      semesterYear: new Date().getFullYear(),
    },
  });

  const handleSubmit = async (data: z.infer<typeof formDataSchema>) => {
    await createCourse(
      new CreateCourseCommand({
        name: data.name,
        description: data.description,
        internalIdentifier: data.internalIdentifier,
        startDate: data.startDate,
        endDate: data.endDate,
        semesterSeason: data.semesterSeason,
        semesterYear: data.semesterYear,
      })
    );
    setOpen(false);
  };

  const years = Array.from(
    { length: 5 },
    (_, i) => new Date().getFullYear() - 2 + i
  );

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button>
          <Plus className="h-4 w-4 mr-2" />
          Add Course
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[600px]">
        <DialogHeader>
          <DialogTitle>Create New Course</DialogTitle>
          <DialogDescription>
            Add a new course to organize your lectures and study materials.
          </DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <FormField
            control={form.control}
            name="name"
            render={({ field }) => (
              <FormItem>
                <FormLabel htmlFor={field.name}>Course Name</FormLabel>
                <FormControl>
                  <Input
                    placeholder="e.g., Introduction to Machine Learning"
                    {...field}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="internalIdentifier"
            render={({ field }) => (
              <FormItem>
                <FormLabel htmlFor={field.name}>Course Code</FormLabel>
                <FormControl>
                  <Input
                    id={field.name}
                    placeholder="e.g., CS-401"
                    {...field}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="description"
            render={({ field }) => (
              <FormItem>
                <FormLabel htmlFor={field.name}>Description</FormLabel>
                <FormControl>
                  <Textarea
                    id={field.name}
                    placeholder="Brief description of the course content and objectives"
                    {...field}
                    rows={3}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <div className="grid grid-cols-2 gap-4">
            <FormField
              control={form.control}
              name="semesterSeason"
              render={({ field }) => (
                <FormItem>
                  <FormLabel htmlFor={field.name}>Semester</FormLabel>
                  <FormControl>
                    <Select
                      value={field.value.toString()}
                      onValueChange={field.onChange}
                    >
                      <SelectTrigger className="w-full">
                        <SelectValue />
                      </SelectTrigger>
                      <SelectContent>
                        {getEnumEntries(Season).map(([key, value]) => (
                          <SelectItem key={key} value={value.toString()}>
                            {key}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="semesterYear"
              render={({ field }) => (
                <FormItem>
                  <FormLabel htmlFor={field.name}>Year</FormLabel>
                  <FormControl>
                    <Select
                      value={field.value.toString()}
                      onValueChange={field.onChange}
                    >
                      <SelectTrigger className="w-full">
                        <SelectValue />
                      </SelectTrigger>
                      <SelectContent>
                        {years.map((year) => (
                          <SelectItem key={year} value={year.toString()}>
                            {year}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <FormField
              control={form.control}
              name="startDate"
              render={({ field }) => (
                <FormItem>
                  <FormLabel htmlFor={field.name}>Start Date</FormLabel>
                  <FormControl>
                    <Input
                      id={field.name}
                      type="date"
                      value={field.value.toISOString().split("T")[0]}
                      onChange={(e) => field.onChange(new Date(e.target.value))}
                      required
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="endDate"
              render={({ field }) => (
                <FormItem>
                  <FormLabel htmlFor={field.name}>End Date</FormLabel>
                  <FormControl>
                    <Input
                      id={field.name}
                      type="date"
                      value={field.value.toISOString().split("T")[0]}
                      onChange={(e) => field.onChange(new Date(e.target.value))}
                      required
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>

          <DialogFooter>
            <Button
              type="button"
              variant="outline"
              onClick={() => setOpen(false)}
            >
              Cancel
            </Button>
            <LoadingButton
              onClick={form.handleSubmit(handleSubmit)}
              loading={isCreating}
              disabled={!form.formState.isValid}
            >
              Create Course
            </LoadingButton>
          </DialogFooter>
        </Form>
      </DialogContent>
    </Dialog>
  );
}
