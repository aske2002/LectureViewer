import ffmpeg from "fluent-ffmpeg";
import path from "path";
import fs from "fs";
import { randomUUID } from "crypto";
import { spawnAsync } from "./spawnAsync";
import { ConvertDocumentRequest } from "../types/convertDocumentRequest";
import { ExtractDocumentThumnailRequest } from "../types/extractDocumentThumnailRequest";
import {
  ExtractDocumentDetailsResponse,
  PageDetails,
  PositionedTextChunk,
} from "../types/extractDocumentDetailsResponse";
import * as pdfjsLib from "pdfjs-dist/legacy/build/pdf.mjs";

export class SofficeService {
  private tempDir = path.join(process.cwd(), "tmp");

  constructor() {
    if (!fs.existsSync(this.tempDir)) {
      fs.mkdirSync(this.tempDir, { recursive: true });
    }
  }

  async getDocumentDetailsWithPdfjs(
    inputPath: string
  ): Promise<ExtractDocumentDetailsResponse> {
    const buffer = await fs.promises.readFile(inputPath);
    const asUint8Array = new Uint8Array(buffer);

    const loadingTask = pdfjsLib.getDocument({ data: asUint8Array });
    const pdf = await loadingTask.promise;

    const numPages = pdf.numPages;

    // Metadata (title/author)
    let title: string | undefined;
    let author: string | undefined;
    try {
      const meta = await pdf.getMetadata().catch(() => null);
      if (meta?.info) {
        if ("Title" in meta.info) title = meta.info.Title as string;
        if ("Author" in meta.info) author = meta.info.Author as string;
      }
    } catch {
      // ignore metadata errors
    }

    const pages: PageDetails[] = [];

    for (let pageNumber = 1; pageNumber <= numPages; pageNumber++) {
      const page = await pdf.getPage(pageNumber);

      const viewport = page.getViewport({ scale: 1 });
      const pageWidth = viewport.width;
      const pageHeight = viewport.height;

      const textContent = await page.getTextContent({
        includeMarkedContent: true,
      });

      const chunks: PositionedTextChunk[] = [];
      let runningIndex = 0;
      let pageTextParts: string[] = [];

      for (const item of textContent.items) {
        // Type guard: only handle text items
        if (!("str" in item)) continue;

        const text = item.str || "";
        if (!text) continue;

        // transform = [a, b, c, d, e, f]
        const [, , , d, e, f] = item.transform;

        // e,f are the x,y translation in PDF coordinate space (origin bottom-left)
        const x = e;
        const y = f;

        const width = item.width;
        const height = Math.abs(d); // d usually encodes font size / height

        const startIndexInPage = runningIndex;
        const endIndexInPage = startIndexInPage + text.length;

        chunks.push({
          text,
          x,
          y,
          width,
          height,
          startIndexInPage,
          endIndexInPage,
        });

        pageTextParts.push(text);
        runningIndex = endIndexInPage;
      }

      const pageText = textContent.items.map((item) =>
            "str" in item ? item.str + (item.hasEOL ? "\n" : "") : ""
          )
          .join("");

      pages.push({
        pageNumber,
        width: pageWidth,
        height: pageHeight,
        text: pageText,
        chunks,
      });
    }

    return {
      numberOfPages: numPages,
      title,
      author,
      pages,
    };
  }

  async convertDocument(
    inputPath: string,
    options: ConvertDocumentRequest
  ): Promise<{
    fileName: string;
    buffer: Buffer;
  }> {
    await spawnAsync(
      "soffice",
      [
        "--headless",
        "--convert-to",
        options.format +
          (options.filterOptions ? `:${options.filterOptions.join(",")}` : ""),
        "--outdir",
        this.tempDir,
        inputPath,
      ],
      {
        pipeStdout: true,
        pipeStderr: true,
      }
    );
    const base = path.basename(inputPath, path.extname(inputPath));
    const convertedPath = path.join(this.tempDir, `${base}.${options.format}`);
    return {
      fileName: `${base}.${options.format}`,
      buffer: await fs.promises.readFile(convertedPath),
    };
  }

  async extractThumbnail(
    inputPath: string,
    options: ExtractDocumentThumnailRequest
  ): Promise<{
    buffer: Buffer;
    fileName: string;
  }> {
    console.log("Extracting thumbnail for", inputPath);
    const args = [
      "--headless",
      "--convert-to",
      "png",
      "--outdir",
      this.tempDir,
      inputPath,
    ];
    await spawnAsync("soffice", args, {
      pipeStdout: true,
      pipeStderr: true,
    });
    const outputBase = path.basename(inputPath);
    const outputPath = path.join(this.tempDir, `${outputBase}.png`);
    return {
      fileName: path.basename(outputPath),
      buffer: await fs.promises.readFile(outputPath),
    };
  }
}
