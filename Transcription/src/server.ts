import express from "express";
import multer from "multer";
import {
  Whisper,
  WhisperLanguagesSchema,
  WhisperModelsSchema,
} from "./utils/whisper";

const app = express();
const upload = multer({ dest: "uploads/"});

app.get("/health", (_, res) => res.json({ status: "ok" }));

app.get("/models", (_, res) => res.json(Whisper.listModels()));

app.get("/languages", (_, res) => res.json(Whisper.listLanguages()));

app.delete("/transcriptions/:id", async (req, res) => {
  const process = Whisper.get(req.params.id)
  if (!process) return res.status(404).send("Transcription not found");
  await process.cancel();
  res.status(200).send("Transcription cancelled");
});

app.post("/detect-language", upload.single("file"), async (req, res) => {
  try {
    if (!req.file) return res.status(400).send("No file uploaded");
    const language = await Whisper.detectLanguage(req.file.path, req.file.originalname);
    res.status(200).send(language);
  } catch (e: any) {
    res.status(500).send(e.toString());
  }
});

app.post("/transcriptions", upload.single("file"), async (req, res) => {
  try {
    if (!req.file) return res.status(400).send("No file uploaded");
    const model = req.body.model
      ? WhisperModelsSchema.parse(req.body.model)
      : undefined;
    const language = req.body.language
      ? WhisperLanguagesSchema.parse(req.body.language)
      : undefined;

    console.log("Starting transcription with model:", model, "and language:", language);

    const id = await Whisper.startTranscription(req.file.path, {
      fileName: req.file.originalname,
      model,
      language,
    });

    res.status(200).send(id);
  } catch (e: any) {
    res.status(500).send(e.toString());
  }
});

app.get("/transcriptions", (req, res) => {
  res.status(200).json(Whisper.list());
});

app.get("/transcriptions/:id", (req, res) => {
  const process = Whisper.get(req.params.id);
  if (!process) return res.status(404).send("Transcription not found");
  res.status(200).json(process.getStatus());
});

app.get("/transcriptions/:id/stream", async (req, res) => {
  const process = Whisper.get(req.params.id);
  if (!process) return res.status(404).send("Transcription not found");

  res.setHeader("Content-Type", "text/event-stream");
  res.setHeader("Cache-Control", "no-cache");
  res.setHeader("Connection", "keep-alive");

  // Immediate hello event
  res.write(`data: ${JSON.stringify({ status: "connected" })}\n\n`);

  process.streamStatus(res);
});

app.listen(3000, () => console.log("Whisper TS service running on :3000"));
