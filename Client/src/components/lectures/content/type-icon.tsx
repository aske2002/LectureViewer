import { LectureContentType } from "@/api/web-api-client";
import {
  FileAudioIcon,
  FileQuestionMarkIcon,
  FileSpreadsheetIcon,
  FileTextIcon,
  FileVideoIcon,
  LucideProps,
} from "lucide-react";

interface LectureContentTypeIconProps extends Omit<LucideProps, "ref"> {
  type: LectureContentType;
}

export const lectureColorClasses: Record<LectureContentType, string> = {
  [LectureContentType.Video]: "text-purple-500",
  [LectureContentType.Document]: "text-green-500",
  [LectureContentType.Audio]: "text-blue-500",
  [LectureContentType.Slides]: "text-orange-500",
  [LectureContentType.Other]: "text-gray-500",
};

export default function LectureContentTypeIcon({
  type,
  ...props
}: LectureContentTypeIconProps) {
  switch (type) {
    case LectureContentType.Video:
      return <FileVideoIcon {...props} />;
    case LectureContentType.Document:
      return <FileTextIcon {...props} />;
    case LectureContentType.Audio:
      return <FileAudioIcon {...props} />;
    case LectureContentType.Slides:
      return <FileSpreadsheetIcon {...props} />;
    default:
      return <FileQuestionMarkIcon {...props} />;
  }
}
