import { z } from "zod";


const TimestampSchema = z.object({
  from: z.string(),
  to: z.string(),
});

const OffsetSchema = z.object({
  from: z.number(),
  to: z.number(),
});

const TokenSchema = z.object({
  text: z.string(),
  timestamps: TimestampSchema,
  offsets: OffsetSchema,
  id: z.number(),
  p: z.number(),
  t_dtw: z.number(),
});

const TranscriptionEntrySchema = z.object({
  timestamps: TimestampSchema,
  offsets: OffsetSchema,
  text: z.string(),
  tokens: z.array(TokenSchema),
});

const ModelSubConfigSchema = z.object({
  ctx: z.number(),
  state: z.number(),
  head: z.number(),
  layer: z.number(),
});

const ModelSchema = z.object({
  type: z.string(),
  multilingual: z.boolean(),
  vocab: z.number(),
  audio: ModelSubConfigSchema,
  text: ModelSubConfigSchema,
  mels: z.number(),
  ftype: z.number(),
});

const ParamsSchema = z.object({
  model: z.string(),
  language: z.string(),
  translate: z.boolean(),
});

const ResultSchema = z.object({
  language: z.string(),
});

export const WhisperOutputSchema = z.object({
  systeminfo: z.string(),
  model: ModelSchema,
  params: ParamsSchema,
  result: ResultSchema,
  transcription: z.array(TranscriptionEntrySchema),
});

export type WhisperOutput = z.infer<typeof WhisperOutputSchema>;
