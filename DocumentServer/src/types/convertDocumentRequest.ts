import z from "zod";

export const ConvertDocumentRequestSchema = z.object({
  format: z.string().optional(),
  filterOptions: z.array(z.string()).optional(),
});

export type ConvertDocumentRequest = z.infer<typeof ConvertDocumentRequestSchema>;