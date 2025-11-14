import { useState } from "react";
import { Loader2, Music } from "lucide-react";
import { FilePreviewHandler } from "@/lib/filePreviews/filePreviewHandler";

interface AudioViewerProps {
  url: string;
  name: string;
  large?: boolean;
}

function AudioViewer({ url, name, large }: AudioViewerProps) {
  const [loading, setLoading] = useState(true);

  return (
    <div
      className={`flex flex-col items-center justify-center rounded-md border bg-muted/20 ${
        large ? "p-6" : "p-3"
      }`}
    >
      {loading && (
        <div className="flex items-center gap-2 text-muted-foreground">
          <Loader2 className="h-4 w-4 animate-spin" />
          <span>Loading audio...</span>
        </div>
      )}
      <audio
        src={url}
        controls
        preload="metadata"
        onLoadedData={() => setLoading(false)}
        className={`w-full ${loading ? "hidden" : "block"}`}
      >
        <track kind="captions" />
        Your browser does not support the audio element.
      </audio>

      {!loading && (
        <div className="flex items-center gap-2 mt-2 text-sm text-muted-foreground">
          <Music className="h-4 w-4" />
          <span className="truncate max-w-[200px]">{name}</span>
        </div>
      )}
    </div>
  );
}

export const audioPreviewHandler: FilePreviewHandler = {
  type: "audio",
  canHandle: (ext, mime) =>
    ["mp3", "wav", "ogg"].includes(ext) || mime?.startsWith("audio/") || false,

  renderInline: ({ url, name }) => <AudioViewer url={url} name={name} />,
  renderModal: ({ url, name }) => <AudioViewer url={url} name={name} large />,
};
