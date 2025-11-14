import JSZip from "jszip";
import { useEffect, useState } from "react";
import { FileArchive } from "lucide-react";
import { FilePreviewHandler } from "@/lib/filePreviews/filePreviewHandler";
import { Spinner } from "@/components/ui/spinner";
import useFormattedSize from "@/hooks/use-formatted-size";

interface ZipViewerProps {
  url: string;
  name: string;
  large?: boolean;
}

function ZipViewer({ url }: ZipViewerProps) {
  const [entries, setEntries] = useState<{
    totalSize: number;
    fileCount: number;
    files: { name: string; dir: boolean; size: number }[];
  } | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;

    async function loadZip() {
      try {
        setLoading(true);
        const res = await fetch(url);
        const blob = await res.blob();

        const zip = await JSZip.loadAsync(blob);
        const newEntries: { name: string; dir: boolean; size: number }[] = [];
        const entries: { relativePath: string; file: JSZip.JSZipObject }[] = [];

        zip.forEach(async (relativePath, file) => {
          entries.push({ relativePath, file });
        });

        for (const { relativePath, file } of entries) {
          if (!file.dir) {
            const unzipped = await file.async("uint8array");
            newEntries.push({
              name: relativePath,
              dir: file.dir,
              size: unzipped.length,
            });
          }
        }

        const totalSize = newEntries.reduce(
          (acc, entry) => acc + entry.size,
          0
        );

        if (!cancelled)
          setEntries({
            totalSize,
            fileCount: newEntries.length,
            files: newEntries,
          });
      } catch (err) {
        if (!cancelled) setError("Failed to read ZIP file.");
        console.error(err);
      } finally {
        if (!cancelled) setLoading(false);
      }
    }

    loadZip();
    return () => {
      console.log("Cancelled zip load");
      cancelled = true;
    };
  }, [url]);

  const formattedSize = useFormattedSize(entries?.totalSize || 0);

  return (
    <>
      {loading && (
        <div className="flex items-center justify-center text-muted-foreground py-6 flex-1 h-full w-full">
          <Spinner className="h-12 w-12 text-primary" />
        </div>
      )}

      {error && !loading && (
        <div className="text-destructive text-sm">{error}</div>
      )}

      {!loading && error == null && entries != null && (
        <div>
          <div className="px-4 py-2 border-b">
            <span className="text-sm text-muted-foreground">
              {entries.fileCount} file
              {entries.fileCount !== 1 ? "s" : ""} ·{" "}
              {formattedSize}
            </span>
          </div>
          {entries.files.map((entry) => (
            <div
              key={entry.name}
              className="flex items-center justify-between px-4 py-2 hover:bg-muted/30 transition border-b last:border-0 cursor-default"
            >
              <div className="flex items-center gap-2 truncate">
                <FileArchive className="h-4 w-4 text-muted-foreground shrink-0" />
                <span className="truncate text-sm">{entry.name}</span>
              </div>
              {!entry.dir && (
                <span className="text-xs text-muted-foreground">
                  {(entry.size / 1024).toFixed(1)} KB
                </span>
              )}
            </div>
          ))}
        </div>
      )}
    </>
  );
}

export const zipPreviewHandler: FilePreviewHandler = {
  type: "zip",
  canHandle: (ext, mime) =>
    ext === "zip" ||
    mime === "application/zip" ||
    mime === "application/x-zip-compressed",

  renderInline: ({ url, name }) => <ZipViewer url={url} name={name} />,
  renderModal: ({ url, name }) => <ZipViewer url={url} name={name} large />,
};
