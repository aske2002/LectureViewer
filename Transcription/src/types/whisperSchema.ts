import { z } from "zod";

export const WhisperSchema = z.object({
    text: z.string(),
    language: z.string(),
    segments: z.array(
        z.object({
            id: z.number(),
            seek: z.number(),
            start: z.number(),
            end: z.number(),
            text: z.string(),
            tokens: z.array(z.number()),
            temperature: z.number(),
            avg_logprob: z.number(),
            compression_ratio: z.number(),
            no_speech_prob: z.number(),
            words: z.array(
                z.object({
                    word: z.string(),
                    start: z.number(),
                    end: z.number(),
                    probability: z.number().optional()
                })
            ).optional()
        })
    )
})

export type WhisperSchema = z.infer<typeof WhisperSchema>;
