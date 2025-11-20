import z from "zod";
import { zodNumberFromString } from "../utils/zodNumberFromString";

export const TranscodeRequestSchema = z.object({
    format: z.string(),
    bitrate: zodNumberFromString.optional(),
    width: zodNumberFromString.optional(),
    height: zodNumberFromString.optional(),
    framerate: zodNumberFromString.optional(),
    audioChannels: zodNumberFromString.optional(),
    audioSampleRate: zodNumberFromString.optional()
});

export type TranscodeRequest = z.infer<typeof TranscodeRequestSchema>;