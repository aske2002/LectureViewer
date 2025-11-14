import { FilePreviewHandler } from "@/lib/filePreviews/filePreviewHandler";
import { FileQuestion } from "lucide-react";

export const defaultPreviewHandler: FilePreviewHandler = {
  type: "default",
  canHandle: () => true,
  renderInline: () => (
    <div className="flex flex-col items-center justify-center text-muted-foreground py-6">
      <FileQuestion className="h-6 w-6 mb-2" />
      <span>No preview available</span>
    </div>
  ),
  renderModal: () => <p>No preview available for this file type.</p>,
};
