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
import { Label } from "@/components/ui/label";
import { Upload, FileAudio, FileVideo, FileText } from "lucide-react";
import { CourseDetailsDto } from "@/api/web-api-client";
import useCourseColor from "@/hooks/use-course-color";
import { cn } from "@/lib/utils";
import { useCourseApi } from "@/api/use-course-api";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import z from "zod";
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
  Form,
} from "../ui/form";
import { Textarea } from "../ui/textarea";
import { LoadingButton } from "../shared/loading-button";

const formDataSchema = z.object({
  title: z.string().min(1, "Lecture title is required"),
  description: z.string().min(1, "Description is required"),
  startDate: z.date(),
  endDate: z.date(),
});

interface CreateLectureDialogProps {
  course: CourseDetailsDto;
}

export function CreateLectureDialog({ course }: CreateLectureDialogProps) {
  const [files, setFiles] = useState<File[]>([]);
  const [isHoveringInput, setIsHoveringInput] = useState(false);

  const courseColor = useCourseColor(course);

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      setFiles(Array.from(e.target.files));
    }
  };

  const [open, setOpen] = useState(false);
  const {
    createLecture: { mutateAsync: createLecture, isPending: isCreating },
  } = useCourseApi(course.id);

  const form = useForm<z.infer<typeof formDataSchema>>({
    resolver: zodResolver(formDataSchema),
    defaultValues: {
      title: "",
      description: "",
      startDate: new Date(),
      endDate: new Date(),
    },
  });

  const handleSubmit = async (data: z.infer<typeof formDataSchema>) => {
    await createLecture({
      id: course.id,
      body: {
        title: data.title,
        description: data.description,
        startDate: data.startDate,
        courseId: course.id,
        endDate: data.endDate,
      },
    });
    setOpen(false);
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button
          style={{ backgroundColor: courseColor.toHex() }}
          onClick={() => setOpen(true)}
        >
          Create Lecture
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[500px]">
        <DialogHeader>
          <DialogTitle>Upload New Lecture</DialogTitle>
          <DialogDescription>
            Upload slides, audio, or video recordings. The system will
            transcribe and analyze the content.
          </DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <FormField
            control={form.control}
            name="title"
            render={({ field }) => (
              <FormItem>
                <FormLabel htmlFor={field.name}>Lecture Title</FormLabel>
                <FormControl>
                  <Input placeholder="Enter lecture title" {...field} />
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
                    placeholder="Brief description of the lecture content and objectives"
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
              name="startDate"
              render={({ field }) => (
                <FormItem>
                  <FormLabel htmlFor={field.name}>
                    Start Date And Time
                  </FormLabel>
                  <FormControl>
                    <Input
                      id={field.name}
                      type="datetime-local"
                      value={field.value.toISOString().slice(0, 16)}
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
                  <FormLabel htmlFor={field.name}>End Date And Time</FormLabel>
                  <FormControl>
                    <Input
                      id={field.name}
                      type="datetime-local"
                      value={field.value.toISOString().slice(0, 16)}
                      onChange={(e) => field.onChange(new Date(e.target.value))}
                      required
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="files">Upload Files</Label>
            <div
              className={cn(
                "border-2 border-dashed border-border rounded-lg p-8 text-center transition-colors"
              )}
              onMouseEnter={() => setIsHoveringInput(true)}
              onMouseLeave={() => setIsHoveringInput(false)}
              style={{
                borderColor: isHoveringInput ? courseColor.toHex() : undefined,
              }}
            >
              <input
                id="files"
                type="file"
                multiple
                accept="audio/*,video/*,.pdf,.ppt,.pptx"
                onChange={handleFileChange}
                className="hidden"
              />
              <label htmlFor="files" className="cursor-pointer">
                <Upload className="h-10 w-10 mx-auto mb-3 text-muted-foreground" />
                <p className="text-sm font-medium mb-1">
                  Click to upload or drag and drop
                </p>
                <p className="text-xs text-muted-foreground">
                  Audio, video, slides (PDF, PPT)
                </p>
              </label>
            </div>
            {files.length > 0 && (
              <div className="mt-4 space-y-2">
                {files.map((file, index) => (
                  <div
                    key={index}
                    className="flex items-center gap-2 text-sm p-2 bg-muted rounded"
                  >
                    {file.type.startsWith("audio/") && (
                      <FileAudio className="h-4 w-4" />
                    )}
                    {file.type.startsWith("video/") && (
                      <FileVideo className="h-4 w-4" />
                    )}
                    {!file.type.startsWith("audio/") &&
                      !file.type.startsWith("video/") && (
                        <FileText className="h-4 w-4" />
                      )}
                    <span className="flex-1 truncate">{file.name}</span>
                    <span className="text-xs text-muted-foreground">
                      {(file.size / 1024 / 1024).toFixed(2)} MB
                    </span>
                  </div>
                ))}
              </div>
            )}
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
              style={{ backgroundColor: courseColor.toHex() }}
            >
              Create Lecture
            </LoadingButton>
          </DialogFooter>
        </Form>
      </DialogContent>
    </Dialog>
  );
}
