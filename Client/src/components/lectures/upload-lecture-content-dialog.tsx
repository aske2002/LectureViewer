import { useEffect, useState } from "react";
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
import {
  CourseDetailsDto,
  LectureDto,
} from "@/api/web-api-client";
import useCourseColor from "@/hooks/use-course-color";
import { LoadingButton } from "../shared/loading-button";
import { FileUploadField, useFileField } from "../shared/file-upload";
import { LectureContentFileUpload } from "./lecture-content-file-upload";
import { getDefaultMetadataForFile } from "@/lib/fileMetadata";
import { Progress } from "../ui/progress";
import { cn } from "@/lib/utils";
import { useLectureApi } from "@/api/use-lecture-api";

interface CreateLectureDialogProps {
  course: CourseDetailsDto;
  lecture: LectureDto;
}

export function UploadLectureContentDialog({
  course,
  lecture,
}: CreateLectureDialogProps) {
  const { invalidateLecture, lecturesClient, axiosInstance } = useLectureApi(
    course.id,
    lecture.id
  );

  const [open, setOpen] = useState(false);
  const [primaryContentFile, setPrimaryContent] = useState<File | null>(null);

  const fileField = useFileField({
    client: lecturesClient,
    axiosInstance,
    mutationFn: async (client, { file, metadata }) => {
      const defaultMetadata = getDefaultMetadataForFile(file);
      return await client.uploadLectureMedia(
        course.id,
        lecture.id,
        metadata?.name || defaultMetadata.name,
        metadata?.description || defaultMetadata.description,
        primaryContentFile === file,
        {
          fileName: file.name,
          data: file,
        }
      );
    },
    onSuccess: async () => {
      await invalidateLecture();
    },
    initialMetadata: getDefaultMetadataForFile,
  });

  useEffect(() => {
    fileField.reset();
  }, [open]);

  const { isPending, upload, progress, files } = fileField;
  const courseColor = useCourseColor(course);

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button
          size={"sm"}
          style={{ backgroundColor: courseColor.toHex() }}
          onClick={() => setOpen(true)}
        >
          Upload Content
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[500px] overflow-hidden max-h-[95vh] flex flex-col">
        <DialogHeader>
          <DialogTitle>Upload Content</DialogTitle>
          <DialogDescription>
            Upload slides, audio, or video recordings. The system will
            transcribe and analyze the content.
          </DialogDescription>
        </DialogHeader>

        <FileUploadField
          className="overflow-y-auto mb-4"
          {...fileField}
          RenderFile={({
            file,
            controller: { remove },
            metadata,
            setMetadata,
          }) => (
            <LectureContentFileUpload
              courseColor={courseColor.toHex()}
              file={file}
              onRemove={remove}
              name={metadata?.name}
              setName={(name) =>
                setMetadata({
                  ...(metadata || getDefaultMetadataForFile(file)),
                  name,
                })
              }
              description={metadata?.description}
              setDescription={(description) =>
                setMetadata({
                  ...(metadata || getDefaultMetadataForFile(file)),
                  description,
                })
              }
              isPrimaryContent={primaryContentFile === file}
              setIsPrimaryContent={(isMain) =>
                setPrimaryContent(isMain ? file : null)
              }
            />
          )}
        />

        <Progress
          value={progress}
          className={cn(
            isPending ? "opacity-100" : "opacity-0",
            "transition-opacity"
          )}
          style={{ backgroundColor: courseColor.lighten(0.4).toHex() }}
          indicatorProps={{ style: { backgroundColor: courseColor.toHex() } }}
        />

        <DialogFooter>
          <Button
            type="button"
            variant="outline"
            onClick={() => setOpen(false)}
          >
            Cancel
          </Button>
          <LoadingButton
            loading={isPending}
            onClick={async () => {
              await upload();
              setOpen(false);
            }}
            disabled={files.length === 0}
            style={{ backgroundColor: courseColor.toHex() }}
          >
            Upload
          </LoadingButton>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
