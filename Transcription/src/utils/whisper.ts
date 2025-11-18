import { ChildProcessWithoutNullStreams, spawn } from "child_process";
import * as os from "os";
import path from "path";
import * as fs from "fs";
import { WhisperOutputSchema, type WhisperOutput } from "../types/whisperSchema";
import z from "zod";
import { randomUUID } from "crypto";
import axios from "axios";

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
      data: WhisperOutput;
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
  public readonly id = randomUUID().replace(/-/g, "");
  private stdoutBuffer: string[] = [];
  private stderrBuffer: string[] = [];

  private result: WhisperOutput | null = null;
  private status: WhisperProcessStatus = WhisperProcessStatus.Running;
  private readonly child: ChildProcessWithoutNullStreams;

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

  private handleSuccess = async () => {
    const outputContent = await fs.promises
      .readFile(this.outPath, "utf8")
      .catch(() => null);

    if (!outputContent) {
      this.status = WhisperProcessStatus.Error;
      this.stderrBuffer.push(
        `Transcription output not found at ${this.outPath}`
      );
      return;
    }

    const result = WhisperOutputSchema.safeParse(JSON.parse(outputContent));

    if (!result.success) {
      this.status = WhisperProcessStatus.Error;
      this.stderrBuffer.push(`Failed to parse transcription output JSON`);
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

    console.log(this.child.spawnargs.join(" "));

    this.child.on("error", (err) => {
      console.error("Whisper process error:", err);
      this.status = WhisperProcessStatus.Error;
      this.stderrBuffer.push(err.toString());
    });

    this.child.stdout.on("data", (data) => {
      console.log(data.toString());
      this.stdoutBuffer.push(data.toString());
    });

    this.child.stderr.on("data", (data) => {
      console.error(data.toString());
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

  public static async run( 
    inputFilePath: string,
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
    console.log(`Requested model: ${model}, found installed models:`, installedModels);
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
    // No-op for now, as models are expected to be pre-installed in the Docker image
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
