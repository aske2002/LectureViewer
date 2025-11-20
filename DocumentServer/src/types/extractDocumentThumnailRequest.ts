import z from "zod";
import { zodNumberFromString } from "../utils/zodNumberFromString";

export const ExtractDocumentThumnailRequestSchema = z.object({
  width: zodNumberFromString.optional(),
  height: zodNumberFromString.optional(),
  pageNumber: zodNumberFromString.optional()
});

export type ExtractDocumentThumnailRequest = z.infer<typeof ExtractDocumentThumnailRequestSchema>;
