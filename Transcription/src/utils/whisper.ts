import { ChildProcessWithoutNullStreams, spawn } from "child_process";
import * as os from "os";
import path from "path";
import * as fs from "fs";
import {
  WhisperLanguage,
  WhisperModel,
  WhisperOutputSchema,
  type WhisperOutput,
} from "../types/whisperSchema";
import z from "zod";
import { randomUUID } from "crypto";
import axios from "axios";
import { Response } from "express-serve-static-core";
import { parseTimecodeToSeconds } from "./numbers";

const progressRegex =
  /whisper_print_progress_callback:\s+progress\s+=\s+(\d+)%/;
const lineRegex =
  /\[(\d{2}:\d{2}:\d{2}\.\d{3})\s+-->\s+(\d{2}:\d{2}:\d{2}\.\d{3})\]\s+(.*)/;

enum WhisperProcessStatus {
  Running = "running",
  Error = "error",
  Success = "success",
}

enum WhisperEventType {
  Transcript = "transcript",
  Progress = "progress",
  Log = "log",
  End = "end",
}

type WhisperProcessResult = { output: string[] } & (
  | {
      data: WhisperOutput;
      status: WhisperProcessStatus.Success;
    }
  | {
      errorMessage: string;
      status: WhisperProcessStatus.Error;
    }
  | {
      status: WhisperProcessStatus.Running;
    }
);

export const WhisperModelsSchema = z.enum(WhisperModel);
export const WhisperLanguagesSchema = z.enum(WhisperLanguage);
export type WhisperEvent = {
  timeStamp: number;
} & (
  | {
      status: WhisperEventType.Transcript;
      from: number;
      to: number;
      text: string;
    }
  | { status: WhisperEventType.Progress; progress: number }
  | { status: WhisperEventType.Log; message: string }
  | { status: WhisperEventType.End }
);

export class WhisperProcess {
  public readonly id = randomUUID().replace(/-/g, "");
  private outputBuffer: { stream: "stderr" | "stdout"; message: string }[] = [];

  private onOutput = (stream: "stderr" | "stdout", message: string) => {
    this.outputBuffer.push({ stream, message });
    this.listeners.forEach((l) => {
      l({
        status: WhisperEventType.Log,
        message: message,
        timeStamp: Date.now(),
      });
    });
  };

  private result: WhisperOutput | null = null;
  private status: WhisperProcessStatus = WhisperProcessStatus.Running;
  private errorMessage: string | null = null;

  private readonly child: ChildProcessWithoutNullStreams;

  private listeners: Array<(data: WhisperEvent) => void> = [];

  private readonly inputFilePath: string;

  private get tmpPath(): string {
    return path.join(os.tmpdir(), "whisper-transcriptions", this.id);
  }

  private get outPathNoExtension(): string {
    return path.join(this.tmpPath, "output");
  }

  private get outPath(): string {
    return `${this.outPathNoExtension}.json`;
  }

  public streamStatus(res: Response<any, Record<string, any>, number>) {
    const listener = (event: WhisperEvent) => {
      res.write(`data: ${JSON.stringify(event)}\n\n`);
      if (event.status === "end") {
        res.end();
      }
    };

    this.listeners.push(listener);

    res.on("close", () => {
      this.listeners = this.listeners.filter((l) => l !== listener);
    });

    if (
      this.status === WhisperProcessStatus.Error ||
      this.status === WhisperProcessStatus.Success
    ) {
      listener({
        status: WhisperEventType.End,
        timeStamp: Date.now(),
      });
    }
  }

  private static get whisperBinary() {
    const appRoot = process.cwd();

    // Possible locations
    const paths = [
      path.join(appRoot, "/whisper/whisper-cli"),
      "/app/whisper/whisper-cli",
    ];

    for (const p of paths) {
      if (fs.existsSync(p)) {
        return p;
      }
    }

    throw new Error("❌ No whisper binary found in expected locations");
  }

  public async cancel() {
    if (this.status === WhisperProcessStatus.Running) {
      return new Promise<void>((resolve) => {
        this.child.on("exit", () => {
          resolve();
        });
        this.child.kill("SIGTERM");
      });
    }
  }

  private finally = async () => {
    this.listeners.forEach((listener) =>
      listener({ status: WhisperEventType.End, timeStamp: Date.now() })
    );
    this.listeners = [];
    fs.rmSync(this.tmpPath, { recursive: true, force: true });
  };

  private handleError = async ({
    message,
    code,
  }: {
    message?: string;
    code?: number;
  }) => {
    this.status = WhisperProcessStatus.Error;
    message =
      message ||
      this.errorMessage ||
      this.outputBuffer.reverse().find((entry) => entry.stream === "stderr")
        ?.message ||
      `Whisper process exited with code ${code}`;
    this.errorMessage = message;
    await this.finally();
  };

  private handleSuccess = async () => {
    const outputContent = await fs.promises
      .readFile(this.outPath, "utf8")
      .catch(() => null);

    if (!outputContent) {
      await this.handleError({
        message: `Transcription output not found at ${this.outPath}`,
      });
      return;
    }

    const result = WhisperOutputSchema.safeParse(JSON.parse(outputContent));

    if (!result.success) {
      await this.handleError({
        message: "Failed to parse transcription output JSON",
      });
      return;
    }

    this.status = WhisperProcessStatus.Success;
    this.result = result.data;
  };

  private constructor(
    inputFilePath: string,
    model: string,
    options: {
      fileName?: string;
      verbose?: boolean;
      temperature?: number;
      language?: WhisperLanguage;
      stdout?: (data: string) => void;
      stderr?: (data: string) => void;
    } = {}
  ) {
    if (!fs.existsSync(this.tmpPath)) {
      fs.mkdirSync(this.tmpPath, { recursive: true });
    }

    this.inputFilePath = path.join(
      this.tmpPath,
      options.fileName || path.basename(inputFilePath)
    );
    fs.copyFileSync(inputFilePath, this.inputFilePath);

    const temperature = options.temperature || 0.0;

    const args = [
      ...(options.language ? ["--language", options.language] : []),
      ...(options.verbose ? ["--verbose", "True"] : []),
      "--print-progress",
      "True",
      "--temperature",
      temperature.toFixed(0).toString(),
      "--model",
      model,
      "--output-file",
      `${this.outPathNoExtension}`,
      "--output-json-full",
      `${this.inputFilePath}`,
    ];

    this.child = spawn(WhisperProcess.whisperBinary, args, {
      stdio: "pipe",
      env: {
        ...process.env,
        PYTHONIOENCODING: "utf-8",
      },
    });

    this.child.on("error", (err) => {
      console.error("Whisper process error:", err);
      this.status = WhisperProcessStatus.Error;
      this.onOutput("stderr", err.toString());
    });

    this.child.stdout.on("data", (data) => {
      console.log(data.toString());
      const lines = String(data)
        .split("\n")
        .filter((line) => line.trim() !== "");
      for (const line of lines) {
        const match = lineRegex.exec(line);
        if (match) {
          const from = parseTimecodeToSeconds(match[1]);
          const to = parseTimecodeToSeconds(match[2]);
          const text = match[3];
          this.listeners.forEach((listener) =>
            listener({
              status: WhisperEventType.Transcript,
              from,
              to,
              text,
              timeStamp: Date.now(),
            })
          );
        }
        this.onOutput("stdout", line);
      }
    });

    this.child.stderr.on("data", (data) => {
      const lines = String(data)
        .split("\n")
        .filter((line) => line.trim() !== "");
      for (const line of lines) {
        if (progressRegex.test(line)) {
          const match = progressRegex.exec(line);
          const progress = match ? parseFloat(match[1]) : 0;
          this.listeners.forEach((listener) =>
            listener({
              status: WhisperEventType.Progress,
              progress,
              timeStamp: Date.now(),
            })
          );
        }
        this.onOutput("stderr", line);
      }
    });

    this.child.on("exit", async (code) => {
      try {
        if (code === 0) {
          await this.handleSuccess();
        } else {
          await this.handleError({ code: code || -1 });
        }
      } finally {
        await this.finally();
      }
    });
  }

  public static async run(
    inputFilePath: string,
    options: {
      fileName?: string;
      verbose?: boolean;
      temperature?: number;
      model?: WhisperModel;
      language?: WhisperLanguage;
    } = {}
  ) {
    const model = await Whisper.getModel(options.model || WhisperModel.LargeV3);
    return new WhisperProcess(inputFilePath, model, options);
  }

  public get process(): ChildProcessWithoutNullStreams {
    return this.child;
  }

  public get pid(): number {
    return this.child.pid || -1;
  }

  public getStatus(): WhisperProcessResult {
    if (this.status === WhisperProcessStatus.Running) {
      return {
        output: this.outputBuffer.map((o) => o.message),
        status: WhisperProcessStatus.Running,
      };
    }

    if (this.status === WhisperProcessStatus.Success && this.result) {
      return {
        output: this.outputBuffer.map((o) => o.message),
        data: this.result,
        status: WhisperProcessStatus.Success,
      };
    }

    return {
      output: this.outputBuffer.map((o) => o.message),
      errorMessage: this.errorMessage || "Unknown error",
      status: WhisperProcessStatus.Error,
    };
  }
}

export class Whisper {
  private static readonly modelsDir = path.join("/app", "models");

  private static readonly runningProcesses: Map<string, WhisperProcess> =
    new Map();

  static listModels(): WhisperModel[] {
    return Object.values(WhisperModel);
  }

  static listLanguages(): WhisperLanguage[] {
    return Object.values(WhisperLanguage);
  }

  public static async getModel(model: WhisperModel): Promise<string> {
    const installedModels = this.listInstalledModels();
    console.log(
      `Requested model: ${model}, found installed models:`,
      installedModels
    );
    if (!installedModels.includes(model)) {
      await this.installModel(model);
    }
    return path.join(this.modelsDir, `${model}.bin`);
  }

  private static listInstalledModels(): WhisperModel[] {
    const modelFiles = fs.existsSync(this.modelsDir)
      ? fs
          .readdirSync(this.modelsDir, { withFileTypes: true })
          .map((dirent) => dirent.name.replace(".bin", ""))
      : [];

    console.log("Model files in models directory:", modelFiles);

    return modelFiles.filter((file) =>
      Object.values(WhisperModel).includes(file as WhisperModel)
    ) as WhisperModel[];
  }

  private static async installModel(model: WhisperModel): Promise<void> {
    console.log(`Installing model: ${model}`);
    const response = await axios.get(
      `https://huggingface.co/ggerganov/whisper.cpp/resolve/main/ggml-${model}.bin`,
      {
        responseType: "stream",
      }
    );
    const modelPath = path.join(this.modelsDir, `${model}.bin`);
    response.data.pipe(fs.createWriteStream(modelPath));

    return new Promise((resolve, reject) => {
      response.data.on("end", () => {
        resolve();
      });
      response.data.on("error", (err: any) => {
        reject(err);
      });
    });
  }

  static async startTranscription(
    filePath: string,
    options: {
      fileName?: string;
      verbose?: boolean;
      temperature?: number;
      model?: WhisperModel;
      language?: WhisperLanguage;
      stdout?: (data: string) => void;
      stderr?: (data: string) => void;
    } = {}
  ) {
    const process = await WhisperProcess.run(filePath, options);
    this.runningProcesses.set(process.id, process);
    return process.id;
  }

  static list(): string[] {
    return Array.from(this.runningProcesses.keys());
  }

  static get(id: string): WhisperProcess | undefined {
    return this.runningProcesses.get(id);
  }
}
