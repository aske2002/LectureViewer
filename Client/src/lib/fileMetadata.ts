import { LectureContentType } from "@/api/web-api-client";

export type LectureContentMetadata = {
  name: string;
  description?: string;
  type: LectureContentType;
};

export function getDefaultMetadataForFile(file: File): LectureContentMetadata {
  return {
    name: file.name,
    description: "",
    type: fileTypeToLectureType(file),
  };
}

export function fileTypeToLectureType(file: File): LectureContentType {
  if (file.type.startsWith("video/")) {
    return LectureContentType.Video;
  }
  if (file.type.startsWith("audio/")) {
    return LectureContentType.Audio;
  }
  if (
    file.type.startsWith("application/vnd.ms-powerpoint") ||
    file.type.startsWith(
      "application/vnd.openxmlformats-officedocument.presentationml"
    )
  ) {
    return LectureContentType.Slides;
  }

  if (
    file.type.startsWith(
      "application/vnd.openxmlformats-officedocument.wordprocessingml"
    ) ||
    file.type.startsWith("application/msword") ||
    file.type.startsWith("application/pdf")
  ) {
    return LectureContentType.Document;
  }

  return LectureContentType.Other;
}
