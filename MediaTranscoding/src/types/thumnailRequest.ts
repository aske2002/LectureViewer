import z from "zod";

export const ThumnailRequestSchema = z.object({
    timeSeconds: z.number().optional(),
    timePercentage: z.number().optional(),
    width: z.number().optional(),
    height: z.number().optional(),
});

export type ThumnailRequest = z.infer<typeof ThumnailRequestSchema>;