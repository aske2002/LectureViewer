import ffmpeg from "fluent-ffmpeg";
import path from "path";
import fs from "fs";
import { randomUUID } from "crypto";
import { spawnAsync } from "./spawnAsync";
import { ConvertDocumentRequest } from "../types/convertDocumentRequest";
import { ExtractDocumentThumnailRequest } from "../types/extractDocumentThumnailRequest";

export class SofficeService {
  private tempDir = path.join(process.cwd(), "tmp");

  constructor() {
    if (!fs.existsSync(this.tempDir)) {
      fs.mkdirSync(this.tempDir, { recursive: true });
    }
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
