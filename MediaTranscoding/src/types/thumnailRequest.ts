import z from "zod";
import { zodNumberFromString } from "../utils/zodNumberFromString";

export const ThumnailRequestSchema = z.object({
    timeSeconds: zodNumberFromString.optional(),
    timePercentage: zodNumberFromString.optional(),
    width: zodNumberFromString.optional(),
    height: zodNumberFromString.optional(),
});

export type ThumnailRequest = z.infer<typeof ThumnailRequestSchema>;