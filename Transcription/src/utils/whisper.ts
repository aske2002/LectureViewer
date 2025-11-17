import { ChildProcessWithoutNullStreams, spawn } from "child_process";
import * as os from "os";
import path from "path";
import * as fs from "fs";
import { WhisperSchema } from "../types/whisperSchema";
import z from "zod";
import { randomUUID } from "crypto";

export enum WhisperLanguage {
  Auto = "auto",
  Afrikaans = "af",
  Amharic = "am",
  Arabic = "ar",
  Assamese = "as",
  Azerbaijani = "az",
  Bashkir = "ba",
  Belarusian = "be",
  Bulgarian = "bg",
  Bengali = "bn",
  Tibetan = "bo",
  Breton = "br",
  Bosnian = "bs",
  Catalan = "ca",
  Czech = "cs",
  Welsh = "cy",
  Danish = "da",
  German = "de",
  Greek = "el",
  English = "en",
  Spanish = "es",
  Estonian = "et",
  Basque = "eu",
  Persian = "fa",
  Finnish = "fi",
  Faroese = "fo",
  French = "fr",
  Galician = "gl",
  Gujarati = "gu",
  Hausa = "ha",
  Hawaiian = "haw",
  Hebrew = "he",
  Hindi = "hi",
  Croatian = "hr",
  HaitianCreole = "ht",
  Hungarian = "hu",
  Armenian = "hy",
  Indonesian = "id",
  Icelandic = "is",
  Italian = "it",
  Japanese = "ja",
  Javanese = "jw",
  Georgian = "ka",
  Kazakh = "kk",
  Khmer = "km",
  Kannada = "kn",
  Korean = "ko",
  Latin = "la",
  Luxembourgish = "lb",
  Lingala = "ln",
  Lao = "lo",
  Lithuanian = "lt",
  Latvian = "lv",
  Malagasy = "mg",
  Maori = "mi",
  Macedonian = "mk",
  Malayalam = "ml",
  Mongolian = "mn",
  Marathi = "mr",
  Malay = "ms",
  Maltese = "mt",
  Burmese = "my",
  Nepali = "ne",
  Dutch = "nl",
  Nynorsk = "nn",
  Norwegian = "no",
  Occitan = "oc",
  Punjabi = "pa",
  Polish = "pl",
  Pashto = "ps",
  Portuguese = "pt",
  Romanian = "ro",
  Russian = "ru",
  Sanskrit = "sa",
  Sindhi = "sd",
  Sinhala = "si",
  Slovak = "sk",
  Slovenian = "sl",
  Shona = "sn",
  Somali = "so",
  Albanian = "sq",
  Serbian = "sr",
  Sundanese = "su",
  Swedish = "sv",
  Swahili = "sw",
  Tamil = "ta",
  Telugu = "te",
  Tajik = "tg",
  Thai = "th",
  Turkmen = "tk",
  Tagalog = "tl",
  Turkish = "tr",
  Tatar = "tt",
  Ukrainian = "uk",
  Urdu = "ur",
  Uzbek = "uz",
  Vietnamese = "vi",
  Yiddish = "yi",
  Yoruba = "yo",
  Cantonese = "yue",
  Chinese = "zh",
}

export enum WhisperModel {
  Tiny = "tiny",
  Base = "base",
  Small = "small",
  Medium = "medium",
  Large = "large",
  LargeV2 = "large-v2",
  LargeV3 = "large-v3",
}

enum WhisperProcessStatus {
  Running = "running",
  Error = "error",
  Success = "success",
}

type WhisperProcessResult =
  | {
      data: WhisperSchema;
      status: WhisperProcessStatus.Success;
    }
  | {
      error: string;
      status: WhisperProcessStatus.Error;
    }
  | {
      stdout: string;
      status: WhisperProcessStatus.Running;
    };

export const WhisperModelsSchema = z.enum(WhisperModel);
export const WhisperLanguagesSchema = z.enum(WhisperLanguage);

export class WhisperProcess {
  public readonly id = randomUUID();
  private stdoutBuffer: string[] = [];
  private stderrBuffer: string[] = [];

  private result: WhisperSchema | null = null;
  private status: WhisperProcessStatus = WhisperProcessStatus.Running;
  private readonly child: ChildProcessWithoutNullStreams;

  private readonly inputFilePath: string;
  private get tmpPath(): string {
    return path.join(os.tmpdir(), "whisper-transcriptions", this.id);
  }

  private static get whisperBinary() {
    const appRoot = process.cwd();

    // Possible locations
    const paths = [
      path.join(appRoot, "whisper-gpu"),
      path.join(appRoot, "whisper"),
      "/app/whisper-gpu",
      "/app/whisper",
    ];

    for (const p of paths) {
      if (fs.existsSync(p)) {
        return p;
      }
    }

    throw new Error("❌ No whisper binary found in expected locations");
  }

  private handleSuccess = async () => {
    const outputPath = path.join(
      this.tmpPath,
      `${path.basename(
        this.inputFilePath,
        path.extname(this.inputFilePath)
      )}.json`
    );

    const outputContent = await fs.promises
      .readFile(outputPath, "utf8")
      .catch(() => null);

    if (!outputContent) {
      this.status = WhisperProcessStatus.Error;
      this.stderrBuffer.push(`Transcription output not found at ${outputPath}`);
      return;
    }

    const result = WhisperSchema.safeParse(JSON.parse(outputContent));

    if (!result.success) {
      this.status = WhisperProcessStatus.Error;
      this.stderrBuffer.push(`Failed to parse transcription output JSON`);
      return;
    }

    this.status = WhisperProcessStatus.Success;
    this.result = result.data;
  };

  public constructor(
    inputFilePath: string,
    options: {
      verbose?: boolean;
      device?: "cpu" | "cuda" | "mps";
      temperature?: number;
      fp16?: boolean;
      model?: WhisperModel;
      wordTimestamps?: boolean;
      language?: WhisperLanguage;
      stdout?: (data: string) => void;
      stderr?: (data: string) => void;
    } = {}
  ) {
    if (!fs.existsSync(this.tmpPath)) {
      fs.mkdirSync(this.tmpPath, { recursive: true });
    }

    this.inputFilePath = path.join(this.tmpPath, path.basename(inputFilePath));
    fs.copyFileSync(inputFilePath, this.inputFilePath);

    const model = options.model || WhisperModel.LargeV3;
    const fp16 = options.fp16 !== undefined ? options.fp16 : false;
    const temperature = options.temperature || 0.0;

    const args = [
      `"${this.inputFilePath}"`,
      ...(options.language ? ["--language", options.language] : []),
      ...(options.device ? ["--device", options.device] : []),
      ...(options.verbose ? ["--verbose", "True"] : []),
      ...(options.wordTimestamps ? ["--word_timestamps", "True"] : []),
      "--temperature",
      temperature.toFixed(0).toString(),
      "--fp16",
      fp16 ? "True" : "False",
      "--model",
      model,
      "--output_dir",
      `"${this.tmpPath}"`,
      "--output_format",
      "json",
    ];

    this.child = spawn(WhisperProcess.whisperBinary, args, {
      stdio: "pipe",
      env: {
        ...process.env,
        PYTHONIOENCODING: "utf-8",
      },
    });

    this.child.on("error", (err) => {
      this.status = WhisperProcessStatus.Error;
      this.stderrBuffer.push(err.toString());
    });

    this.child.stdout.on("data", (data) => {
      this.stdoutBuffer.push(data.toString());
    });

    this.child.stderr.on("data", (data) => {
      this.stderrBuffer.push(data.toString());
    });

    this.child.on("exit", (code) => {
      if (code === 0) {
        this.handleSuccess();
      } else {
        this.status = WhisperProcessStatus.Error;
      }
    });
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
        stdout: this.stdoutBuffer.join("\n"),
        status: WhisperProcessStatus.Running,
      };
    }

    if (this.status === WhisperProcessStatus.Success && this.result) {
      return {
        data: this.result,
        status: WhisperProcessStatus.Success,
      };
    }

    return {
      error: this.stderrBuffer.join("\n"),
      status: WhisperProcessStatus.Error,
    };
  }
}

export class Whisper {
  private static readonly runningProcesses: Map<string, WhisperProcess> =
    new Map();

  static listModels(): WhisperModel[] {
    return Object.values(WhisperModel);
  }

  static listLanguages(): WhisperLanguage[] {
    return Object.values(WhisperLanguage);
  }

  static startTranscription(
    filePath: string,
    options: {
      verbose?: boolean;
      device?: "cpu" | "cuda" | "mps";
      temperature?: number;
      fp16?: boolean;
      model?: WhisperModel;
      wordTimestamps?: boolean;
      language?: WhisperLanguage;
      stdout?: (data: string) => void;
      stderr?: (data: string) => void;
    } = {}
  ) {
    const process = new WhisperProcess(filePath, options);
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
