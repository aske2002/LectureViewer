import { LectureContentDto } from "@/api/web-api-client";
import LectureContentTypeIcon, { lectureColorClasses } from "./type-icon";
import { Card } from "@/components/ui/card";
import {
  ArrowDown,
  ArrowUp,
  ArrowUpDownIcon,
  Calendar,
  Download,
} from "lucide-react";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import useFormattedSize from "@/hooks/use-formatted-size";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { useMemo, useState } from "react";
import FilePreview from "@/components/shared/filePreview/file-preview";

interface LectureContentSectionProps {
  courseId: string;
  lectureId: string;
  contents: LectureContentDto[];
}

export default function LectureContentSection({
  contents,
  courseId,
  lectureId,
}: LectureContentSectionProps) {
  const [sortingMethod, setSortingMethod] = useState<
    "date" | "size" | "name" | "type"
  >("date");
  const [sortDirection, setSortDirection] = useState<"asc" | "desc">("desc");

  const handleSortChange = (method: "date" | "size" | "name" | "type") => {
    if (sortingMethod === method) {
      setSortDirection(sortDirection === "asc" ? "desc" : "asc");
    } else {
      setSortingMethod(method);
      setSortDirection("asc");
    }
  };

  const sortedContents = useMemo(() => {
    const sorted = [...contents];
    sorted.sort((a, b) => {
      let comparison = 0;
      switch (sortingMethod) {
        case "name":
          comparison = a.name.localeCompare(b.name);
          break;
        case "date":
          comparison =
            new Date(a.created).getTime() - new Date(b.created).getTime();
          break;
        case "size":
          comparison = a.resource.size - b.resource.size;
          break;
        case "type":
          comparison = a.contentType.localeCompare(b.contentType);
          break;
      }
      return sortDirection === "asc" ? comparison : -comparison;
    });
    return sorted;
  }, [contents, sortingMethod, sortDirection]);

  return (
    <div>
      <div>
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="outline" className="mb-4">
              <ArrowUpDownIcon />
              Sort By
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent>
            <DropdownMenuItem
              onClick={() => handleSortChange("date")}
              className="flex items-center justify-between"
            >
              Date Added
              {sortingMethod === "date" ? (
                sortDirection === "asc" ? (
                  <ArrowUp className="h-4 w-4 mr-2" />
                ) : (
                  <ArrowDown className="h-4 w-4 mr-2" />
                )
              ) : null}
            </DropdownMenuItem>
            <DropdownMenuItem
              onClick={() => handleSortChange("name")}
              className="flex items-center justify-between"
            >
              Name
              {sortingMethod === "name" ? (
                sortDirection === "asc" ? (
                  <ArrowUp className="h-4 w-4 mr-2" />
                ) : (
                  <ArrowDown className="h-4 w-4 mr-2" />
                )
              ) : null}
            </DropdownMenuItem>
            <DropdownMenuItem
              onClick={() => handleSortChange("size")}
              className="flex items-center justify-between"
            >
              Size
              {sortingMethod === "size" ? (
                sortDirection === "asc" ? (
                  <ArrowUp className="h-4 w-4 mr-2" />
                ) : (
                  <ArrowDown className="h-4 w-4 mr-2" />
                )
              ) : null}
            </DropdownMenuItem>
            <DropdownMenuItem
              onClick={() => handleSortChange("type")}
              className="flex items-center justify-between"
            >
              Type
              {sortingMethod === "type" ? (
                sortDirection === "asc" ? (
                  <ArrowUp className="h-4 w-4 mr-2" />
                ) : (
                  <ArrowDown className="h-4 w-4 mr-2" />
                )
              ) : null}
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {sortedContents.map((content) => {
          return (
            <LectureContentSectionItem
              key={content.id}
              content={content}
              courseId={courseId}
              lectureId={lectureId}
            />
          );
        })}
      </div>
    </div>
  );
}

const documentMimeTypes = [
  "application/msword",
  "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
  "application/vnd.ms-powerpoint",
  "application/vnd.openxmlformats-officedocument.presentationml.presentation",
  "application/vnd.ms-excel",
  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
];

interface LectureContentSectionItemProps {
  content: LectureContentDto;
  courseId: string;
  lectureId: string;
}

function LectureContentSectionItem({
  content,
  courseId,
  lectureId,
}: LectureContentSectionItemProps) {
  const [previewOpen, setPreviewOpen] = useState(false);

  const {
    name,
    resource: { fileName },
  } = content;

  const { mimeType, id: resourceId } = useMemo(() => {
    if (documentMimeTypes.includes(content.resource.mimeType)) {
      return (
        content.resource.associatedResources?.find(
          (res) => res.mimeType === "application/pdf"
        ) || content.resource
      );
    }
    return content.resource;
  }, [content]);

  const { url, downloadUrl } = useMemo(
    () => ({
      url: `/api/Lectures/${courseId}/${lectureId}/contents/${content.id}/${resourceId}`,
      downloadUrl: `/api/Lectures/${courseId}/${lectureId}/contents/${content.id}/${resourceId}?download=true`,
    }),
    [courseId, lectureId, content, resourceId]
  );

  const colorClass = lectureColorClasses[content.contentType];
  const size = useFormattedSize(content.resource.size);

  return (
    <>
      <FilePreview
        open={previewOpen}
        onOpenChange={setPreviewOpen}
        fileName={fileName}
        mimeType={mimeType}
        url={url}
        downloadUrl={downloadUrl}
        title={name}
      />
      <Card
        key={content.id}
        onClick={() => setPreviewOpen(true)}
        className="p-4 hover:border-primary transition-colors w-full"
      >
        <div className="flex items-start gap-3">
          <div className={`${colorClass} mt-1 shrink-0`}>
            <LectureContentTypeIcon
              type={content.contentType}
              className="h-6 w-6"
            />
          </div>
          <div className="flex-1 min-w-0">
            <div className="flex items-center gap-2 mb-1">
              <h3 className="font-medium text-sm truncate">{content.name}</h3>
            </div>
            {content.description && (
              <p className="text-xs text-muted-foreground mb-3 line-clamp-2">
                {content.description}
              </p>
            )}
            <div className="flex items-center gap-2 text-xs text-muted-foreground mb-3">
              <Calendar className="h-3 w-3" />
              <span>{new Date(content.created).toLocaleDateString()}</span>
              <span>•</span>
              <span>{size}</span>
            </div>
            <div className="flex items-center gap-2">
              <Badge variant="outline" className="text-xs capitalize">
                {content.contentType}
              </Badge>
              <Button size="sm" variant="ghost" className="h-7 px-2 ml-auto">
                <Download className="h-3 w-3 mr-1" />
                Download
              </Button>
            </div>
          </div>
        </div>
      </Card>
    </>
  );
}
