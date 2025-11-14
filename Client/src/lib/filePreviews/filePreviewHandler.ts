import { ReactNode } from "react";
import { registerHandlers } from "../../components/shared/filePreview/handlers";

export interface FilePreviewContext {
  url: string;
  name: string;
  maxSize: {
    width: number;
    height: number;
  }
  mimeType?: string;
  extension?: string;
}

export interface FilePreviewHandler {
  /** A unique identifier for this handler (e.g. "pdf", "image", "video") */
  type: string;

  /** Whether this handler can preview the given file */
  canHandle: (ext: string, mimeType?: string) => boolean;

  /** Render small inline preview (used in cards, lists, etc.) */
  renderInline: (ctx: FilePreviewContext) => ReactNode;

  /** Render full modal preview */
  renderModal: (ctx: FilePreviewContext) => ReactNode;
}

const handlers: FilePreviewHandler[] = [];

export function registerPreviewHandler(handler: FilePreviewHandler) {
  handlers.push(handler);
}

export function getPreviewHandler(ext: string, mimeType?: string): FilePreviewHandler | undefined {
  return handlers.find((h) => h.canHandle(ext, mimeType));
}

export function getAllPreviewHandlers() {
  return handlers;
}

registerHandlers();