import { useEffect, useMemo, useState } from "react";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { getPreviewHandler } from "@/lib/filePreviews/filePreviewHandler";
import { Download, XIcon } from "lucide-react";
import { Button } from "@/components/ui/button";

interface FilePreviewProps {
  fileName: string;
  downloadUrl?: string;
  url: string;
  mimeType?: string;
  title?: string;
  open?: boolean;
  onOpenChange?: (open: boolean) => void;
}

export default function FilePreview({
  fileName,
  url,
  downloadUrl,
  title,
  mimeType,
  open,
  onOpenChange,
}: FilePreviewProps) {
  const [dialogContent, setDialogContentRef] = useState<HTMLDivElement | null>(
    null
  );
  const [maxWidth, setMaxWidth] = useState<number>(0);
  const [maxHeight, setMaxHeight] = useState<number>(0);

  useEffect(() => {
    if (!dialogContent) return;

    const updateMaxWidth = () => {
      const { width = 0, height = 0 } = dialogContent.getBoundingClientRect();
      setMaxWidth(width);
      setMaxHeight(height);
    };
    updateMaxWidth();

    window.addEventListener("resize", updateMaxWidth);
    return () => window.removeEventListener("resize", updateMaxWidth);
  }, [dialogContent]);

  const ext = useMemo(
    () => fileName.split(".").pop()?.toLowerCase() || "",
    [fileName]
  );

  const handler = useMemo(
    () => getPreviewHandler(ext, mimeType),
    [ext, mimeType]
  );

  if (!handler) return null;

  return (
    <>
      <Dialog open={open} onOpenChange={onOpenChange}>
        <DialogContent
          showCloseButton={false}
          className="w-full max-w-full! h-full flex flex-col overflow-hidden rounded-none bg-black/30 border-none p-0 gap-0"
        >
          <DialogHeader className="px-4 pt-2 flex flex-row items-center justify-between text-white gap-4">
            <div>
              <p className="font-semibold text-lg">{title}</p>
              <p className="text-sm text-neutral-200">{fileName}</p>
            </div>
            <div className="flex-1" />

            {downloadUrl && (
              <a href={downloadUrl} download>
                <Button size={"icon"} variant={"secondary"}>
                  <Download />
                </Button>
              </a>
            )}
            <Button size={"icon"} variant="ghost" asChild>
              <DialogClose>
                <XIcon />
              </DialogClose>
            </Button>
          </DialogHeader>
          <div
            className="flex h-full flex-1 flex-col overflow-hidden p-4"
            ref={setDialogContentRef}
          >
            <div className="w-full rounded-2xl shadow-sm bg-white overflow-y-auto flex-1">
              {handler.renderModal({
                name: fileName,
                url,
                mimeType,
                extension: ext,
                maxSize: { width: maxWidth, height: maxHeight },
              })}
            </div>
          </div>
        </DialogContent>
      </Dialog>
    </>
  );
}
