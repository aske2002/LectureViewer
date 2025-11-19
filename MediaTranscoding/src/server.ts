import multer from "multer";
import { TranscoderService } from "./utils/ffmpeg";
import express from "express";
import { requestWithValidation } from "./utils/requestWithValidation";
import { TranscodeRequestSchema } from "./types/transcodeRequest";
import { ThumnailRequestSchema } from "./types/thumnailRequest";

const app = express();
const upload = multer({ dest: "uploads/" });
const transcoder = new TranscoderService();

app.post(
  "/transcode",
  upload.single("file"),
  requestWithValidation(TranscodeRequestSchema, async (body, req, res) => {
    try {
      if (!req.file) return res.status(400).send("No file uploaded");

      const outputPath = await transcoder.transcodeFile(req.file.path, {
        ...body,
        onProgress: (p) => console.log("Progress:", p.percent),
      });

      res.send(outputPath);
    } catch (err: any) {
      console.error(err);
      res.status(500).json({ error: err.message });
    }
  })
);

app.post(
  "/thumbnail",
  upload.single("file"),
  requestWithValidation(ThumnailRequestSchema, async (body, req, res) => {
    try {
      if (!req.file) return res.status(400).send("No file uploaded");

      const outputPath = await transcoder.generateThumbnail(req.file.path, body);

      res.send(outputPath);
    } catch (err: any) {
      console.error(err);
      res.status(500).json({ error: err.message });
    }
  })
);

app.listen(3000, () => console.log("Transcoder Service running on :3000"));
