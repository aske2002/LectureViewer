import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { FileText, X } from "lucide-react";

interface LectureContentFileUploadProps {
  courseColor: string;
  file: File;
  onRemove: () => void;
  name?: string;
  setName: (name: string) => void;
  description?: string;
  setDescription: (description: string) => void;
  isPrimaryContent: boolean;
  setIsPrimaryContent: (isMain: boolean) => void;
}

export function LectureContentFileUpload({
  file,
  name,
  description,
  courseColor,
  onRemove,
  setName,
  setDescription,
  isPrimaryContent,
  setIsPrimaryContent,
}: LectureContentFileUploadProps) {
  const [isHovered, setIsHovered] = useState(false);

  const handleClick = (e: React.MouseEvent<HTMLDivElement>) => {
    if (e.target === e.currentTarget) {
      setIsPrimaryContent(!isPrimaryContent);
    }
  };

  return (
    <div
      className="border rounded-lg p-4 bg-muted/30"
      onMouseEnter={() => setIsHovered(true)}
      onMouseLeave={() => setIsHovered(false)}
    >
      <div className="flex items-start gap-3" onClick={handleClick}>
        <button
          onClick={() => {
            setIsPrimaryContent(!isPrimaryContent);
          }}
          type="button"
          className="flex flex-col items-center gap-1 pt-1 group shrink-0"
          title="Mark as main content"
        >
          <div className="relative flex h-6 w-6 items-center justify-center">
            <div
              className="h-6 w-6 rounded-full border-2 transition-all border-input"
              style={{
                borderColor:
                  isPrimaryContent || isHovered ? courseColor : undefined,
              }}
            />
            {isPrimaryContent && (
              <div
                className="absolute h-3 w-3 rounded-full bg-current"
                style={{ backgroundColor: courseColor }}
              />
            )}
          </div>
          <span
            className="text-[10px] font-medium transition-colors text-muted-foreground"
            style={{
              color: isPrimaryContent || isHovered ? courseColor : undefined,
            }}
          >
            Primary
          </span>
        </button>

        <FileText className="h-5 w-5 mt-1 shrink-0 text-muted-foreground" />

        <div className="flex-1 min-w-0 space-y-3">
          <div className="flex items-center gap-2">
            <Input
              placeholder="Material name"
              value={name}
              onChange={(e) => setName(e.target.value)}
              className="h-8 text-sm flex-1"
            />
            <Button
              type="button"
              variant="ghost"
              size="sm"
              onClick={onRemove}
              className="h-8 w-8 p-0 shrink-0"
            >
              <X className="h-4 w-4" />
            </Button>
          </div>
          <p className="text-xs text-muted-foreground truncate">
            {file.name} ({(file.size / 1024 / 1024).toFixed(2)} MB)
          </p>
          <Textarea
            placeholder="Description (optional)"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            rows={2}
            className="text-sm"
          />
        </div>
      </div>
    </div>
  );
}
