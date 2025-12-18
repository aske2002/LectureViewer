import { Router } from "express";
import multer from "multer";
import { SofficeService } from "./utils/soffice";
import express from "express";
import { requestWithValidation } from "./utils/requestWithValidation";
import { ConvertDocumentRequestSchema } from "./types/convertDocumentRequest";
import { ExtractDocumentThumnailRequestSchema } from "./types/extractDocumentThumnailRequest";

const app = express();
const upload = multer({ dest: "uploads/" });
const officeService = new SofficeService();

app.post(
  "/convert",
  upload.single("file"),
  requestWithValidation(
    ConvertDocumentRequestSchema,
    async (body, req, res) => {
      try {
        if (!req.file) return res.status(400).send("No file uploaded");
        const { buffer, fileName } = await officeService.convertDocument(
          req.file.path,
          body
        );
        res.setHeader(
          "Content-Disposition",
          `attachment; filename="${fileName}"`
        );
        res.send(buffer);
      } catch (error) {
        console.error("Conversion error:", error);
        res.status(500).send("Conversion failed");
      }
    }
  )
);

app.post(
  "/details",
  upload.single("file"),
  async (req, res) => {
    try {
      if (!req.file) return res.status(400).send("No file uploaded");
      const details = await officeService.getDocumentDetailsWithPdfjs(req.file.path);
      res.json(details);
    } catch (error) {
      console.error("Get details error:", error);
      res.status(500).send("Failed to get document details");
    }
  }
);

app.post(
  "/thumbnail",
  upload.single("file"),
  requestWithValidation(
    ExtractDocumentThumnailRequestSchema,
    async (body, req, res) => {
      try {
        if (!req.file) return res.status(400).send("No file uploaded");
        const { buffer, fileName } = await officeService.extractThumbnail(
          req.file.path,
          body
        );
        res.setHeader(
          "Content-Disposition",
          `attachment; filename="${fileName}"`
        );
        res.send(buffer);
      } catch (error) {
        console.error("Thumbnail extraction error:", error);
        res.status(500).send("Thumbnail extraction failed");
      }
    }
  )
);

app.listen(3000, () => console.log("Document Server running on :3000"));
