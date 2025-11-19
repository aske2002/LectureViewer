import z from "zod";

export const ExtractDocumentThumnailRequestSchema = z.object({
  width: z.number().optional(),
  height: z.number().optional(),
  pageNumber: z.number().min(1).optional(),
});

export type ExtractDocumentThumnailRequest = z.infer<typeof ExtractDocumentThumnailRequestSchema>;
