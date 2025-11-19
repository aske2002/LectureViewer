import ffmpeg, { ffprobe } from "fluent-ffmpeg";
import path from "path";
import fs, { readFileSync } from "fs";
import { randomUUID } from "crypto";
import { TranscodeRequest } from "../types/transcodeRequest";
import { ThumnailRequest } from "../types/thumnailRequest";

export type TranscodeOptions = TranscodeRequest & {
  onProgress?: (p: { percent: number }) => void;
};

export class TranscoderService {
  private tempDir = path.join(process.cwd(), "tmp");

  constructor() {
    if (!fs.existsSync(this.tempDir)) {
      fs.mkdirSync(this.tempDir, { recursive: true });
    }
  }

  /**
   * Transcodes a media file using ffmpeg
   */
  async transcodeFile(
    inputPath: string,
    options: TranscodeOptions
  ): Promise<Buffer> {
    return new Promise((resolve, reject) => {
      const outputFile = path.join(
        this.tempDir,
        `${randomUUID()}.${options.format}`
      );

      const command = ffmpeg(inputPath).output(outputFile);

      const isAudioOnly = ["mp3", "wav", "aac", "flac", "ogg"].includes(
        options.format
      );

      if (!isAudioOnly) {
        if (options.bitrate) {
          command.videoBitrate(options.bitrate);
        }
        if (options.width && options.height) {
          command.size(`${options.width}x${options.height}`);
        }
        if (options.framerate) {
          command.fps(options.framerate);
        }
      }

      if (options.audioChannels) {
        command.audioChannels(options.audioChannels);
      }
      if (options.audioSampleRate) {
        command.audioFrequency(options.audioSampleRate);
      }

      command
        .on("progress", (progress) => {
          if (options.onProgress) {
            options.onProgress({ percent: progress.percent || 0 });
          }
        })
        .on("end", () => {
          resolve(readFileSync(outputFile));
        })
        .on("error", (err) => {
          reject(err);
        });

      command.run();
    });
  }

  async generateThumbnail(
    inputPath: string,
    options: ThumnailRequest
  ): Promise<Buffer> {
    return new Promise(async (resolve, reject) => {
      const outputFile = path.join(this.tempDir, `${randomUUID()}.png`);
      const mediaDuration = await new Promise<number>((res, rej) => {
        ffprobe(inputPath, (err, metadata) => {
          if (err) {
            rej(err);
            return;
          }
          const duration = metadata.format.duration || 0;
          res(duration);
        });
      });

      let timeSeconds = 0;
      if (options.timeSeconds !== undefined) {
        timeSeconds = options.timeSeconds;
      } else if (options.timePercentage !== undefined) {
        timeSeconds = (options.timePercentage / 100) * mediaDuration;
      }

      ffmpeg(inputPath)
        .screenshots({
          timestamps: [timeSeconds],
          filename: path.basename(outputFile),
          folder: this.tempDir,
          size:
            options.width && options.height
              ? `${options.width}x${options.height}`
              : "320x240",
        })
        .on("end", () => {
          resolve(readFileSync(outputFile));
        })
        .on("error", (err) => {
          reject(err);
        });
    });
  }
}
