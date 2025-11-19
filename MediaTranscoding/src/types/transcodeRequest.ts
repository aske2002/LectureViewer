import z from "zod";

export const TranscodeRequestSchema = z.object({
    format: z.string(),
    bitrate: z.number().optional(),
    width: z.number().optional(),
    height: z.number().optional(),
    framerate: z.number().optional(),
    audioChannels: z.number().optional(),
    audioSampleRate: z.number().optional(),
});

export type TranscodeRequest = z.infer<typeof TranscodeRequestSchema>;