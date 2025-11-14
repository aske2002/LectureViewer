import { useState } from "react";
import { Loader2 } from "lucide-react";
import { FilePreviewHandler } from "@/lib/filePreviews/filePreviewHandler";

interface VideoViewerProps {
  url: string;
  name: string;
  large?: boolean;
}

function VideoViewer({ url, name, large }: VideoViewerProps) {
  const [loading, setLoading] = useState(true);

  return (
    <div
      className={`relative bg-black rounded-md overflow-hidden ${
        large ? "max-h-[80vh]" : "max-h-48"
      }`}
    >
      {loading && (
        <div className="absolute inset-0 flex items-center justify-center text-muted-foreground">
          <Loader2 className="h-5 w-5 animate-spin" />
        </div>
      )}

      <video
        src={url}
        controls
        playsInline
        preload="metadata"
        onLoadedData={() => setLoading(false)}
        className={`w-full h-auto object-contain transition-opacity duration-300 ${
          loading ? "opacity-0" : "opacity-100"
        }`}
      >
        <track kind="captions" />
        Sorry, your browser doesn’t support embedded videos.
      </video>
    </div>
  );
}

export const videoPreviewHandler: FilePreviewHandler = {
  type: "video",
  canHandle: (ext, mime) =>
    ["mp4", "webm", "mov"].includes(ext) ||
    mime?.startsWith("video/") ||
    mime === "application/octet-stream", // optional fallback

  renderInline: ({ url, name }) => <VideoViewer url={url} name={name} />,
  renderModal: ({ url, name }) => <VideoViewer url={url} name={name} large />,
};
