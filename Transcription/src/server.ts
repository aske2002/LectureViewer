import express from "express";
import multer from "multer";
import {
  Whisper,
  WhisperLanguagesSchema,
  WhisperModelsSchema,
} from "./utils/whisper";

const app = express();
const upload = multer({ dest: "uploads/" });

app.get("/health", (_, res) => res.json({ status: "ok" }));

app.get("/models", (_, res) => res.json(Whisper.listModels()));

app.get("/languages", (_, res) => res.json(Whisper.listLanguages()));

app.post("/transcriptions", upload.single("file"), async (req, res) => {
  try {
    if (!req.file) return res.status(400).send("No file uploaded");
    const model = req.query.model
      ? WhisperModelsSchema.parse(req.query.model)
      : undefined;
    const language = req.query.language
      ? WhisperLanguagesSchema.parse(req.query.language)
      : undefined;

    const id = Whisper.startTranscription(req.file.path, {
      model,
      language,
    });

    res.status(200).json(id);
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

app.listen(3000, () => console.log("Whisper TS service running on :3000"));
