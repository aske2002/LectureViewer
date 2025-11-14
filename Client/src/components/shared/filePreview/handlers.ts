import { imagePreviewHandler } from "@/components/shared/filePreview/handlers/image";
import { registerPreviewHandler } from "../../../lib/filePreviews/filePreviewHandler";
import { pdfPreviewHandler } from "@/components/shared/filePreview/handlers/pdf";
import { defaultPreviewHandler } from "@/components/shared/filePreview/handlers/default";
import { zipPreviewHandler } from "./handlers/zip";
import { mediaPreviewHandler } from "./handlers/media";
import { model3dPreviewHandler } from "./handlers/3d";

export const registerHandlers = () => {
  registerPreviewHandler(imagePreviewHandler);
  registerPreviewHandler(pdfPreviewHandler);
  registerPreviewHandler(zipPreviewHandler);
  registerPreviewHandler(mediaPreviewHandler);
  registerPreviewHandler(model3dPreviewHandler);

  registerPreviewHandler(defaultPreviewHandler);
};
