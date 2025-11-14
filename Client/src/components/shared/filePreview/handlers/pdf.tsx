import { FilePreviewHandler } from "@/lib/filePreviews/filePreviewHandler";
import { useState, useCallback, useEffect, useRef, useMemo } from "react";
import { Document, Page, pdfjs } from "react-pdf";
import {
  ZoomIn,
  ZoomOut,
  ChevronLeft,
  ChevronRight,
  Loader2,
  RefreshCw,
} from "lucide-react";
import { Input } from "@/components/ui/input";
import { cn } from "@/lib/utils";
import useTheme from "@/hooks/use-theme";
import { OnItemClickArgs } from "react-pdf/dist/shared/types.js";
import { Button } from "@/components/ui/button";

interface PdfViewerProps {
  url: string;
  name: string;
}

interface PdfViewerProps {
  url: string;
}

function highlightPattern(text: string, pattern: RegExp, background: string) {
  return text.replace(
    pattern,
    (value) => `<mark style="background: ${background}">${value}</mark>`
  );
}

function PdfViewer({ url }: PdfViewerProps) {
  const [numPages, setNumPages] = useState<number>(0);
  const [pageNumber, setPageNumber] = useState(1);
  const [requestedPageNumber, setRequestedPageNumber] = useState(1);

  const [scale, setScale] = useState(1.0);
  const [loading, setLoading] = useState(true);
  const containerRef = useRef<HTMLDivElement>(null);

  const [size, setContainerSize] = useState<{
    width: number;
    height: number;
  } | null>(null);

  useEffect(() => {
    if (!containerRef.current) return;

    const updateSize = () => {
      const size = containerRef.current?.getBoundingClientRect() || null;
      setContainerSize(size);
    };
    updateSize();

    window.addEventListener("resize", updateSize);
    return () => window.removeEventListener("resize", updateSize);
  }, [containerRef]);

  const [pdfDoc, setPdfDoc] = useState<pdfjs.PDFDocumentProxy | null>(null);
  const [page, setPage] = useState<pdfjs.PDFPageProxy | null>(null);

  // --- Search state ---
  const [searchOpen, setSearchOpen] = useState(false);
  const searchRef = useRef<HTMLDivElement>(null);
  const [query, setQuery] = useState("");
  const [matches, setMatches] = useState<
    | {
        search: string;
        matches: { page: number; text: string }[];
      }
    | undefined
  >();
  const [matchIndex, setMatchIndex] = useState(0);
  const handleDocumentLoad = useCallback((pdf: pdfjs.PDFDocumentProxy) => {
    setNumPages(pdf.numPages);
    setPdfDoc(pdf);
    setLoading(false);
  }, []);

  useEffect(() => {
    if (requestedPageNumber != pageNumber) {
      setRequestedPageNumber(pageNumber);
    }
  }, [pageNumber]);

  useEffect(() => {
    if (requestedPageNumber !== pageNumber) {
      setPageNumber(requestedPageNumber);
    }
  }, [requestedPageNumber]);

  const highlightColor = useTheme().primary.withAlpha(0.3).toHex();

  const zoomIn = () => setScale((s) => Math.min(s + 0.2, 3));
  const zoomOut = () => setScale((s) => Math.max(s - 0.2, 0.4));
  const resetZoom = () => setScale(1.0);
  const nextPage = () => setPageNumber((p) => Math.min(p + 1, numPages));
  const prevPage = () => setPageNumber((p) => Math.max(p - 1, 1));

  const pageSize = useMemo(() => {
    if (!page || !size) return null;
    const pageAspect =
      page.getViewport({ scale: 1 }).width /
      page.getViewport({ scale: 1 }).height;
    const containerAspect = size.width / size.height;

    let adjustedWidth = 0;
    let adjustedHeight = 0;

    if (size.width && pageAspect > containerAspect) {
      adjustedWidth = size.width;
      adjustedHeight = adjustedWidth / pageAspect;
    } else {
      adjustedHeight = size.height;
      adjustedWidth = adjustedHeight * pageAspect;
    }

    return { pageWidth: adjustedWidth, pageHeight: adjustedHeight };
  }, [page, size]);

  // --- Search logic ---
  const handleSearch = useCallback(async () => {
    if (!pdfDoc || !query.trim()) {
      setMatches(undefined);
      return;
    }

    if (matches?.search === query) {
      nextMatch();
      return;
    }

    const lower = query.toLowerCase();
    const results: { page: number; text: string }[] = [];

    for (let i = 1; i <= pdfDoc.numPages; i++) {
      const page = await pdfDoc.getPage(i);
      const textContent = await page.getTextContent();
      const pageText = textContent.items.map((item: any) => item.str).join(" ");
      if (pageText.toLowerCase().includes(lower)) {
        results.push({ page: i, text: pageText });
      }
    }

    setMatches({ search: query, matches: results });
    if (results.length > 0) {
      setPageNumber(results[0].page);
      setMatchIndex(0);
    }
  }, [pdfDoc, query, matches, matchIndex]);

  const nextMatch = () => {
    if (!matches?.matches.length) return;
    const next = (matchIndex + 1) % matches.matches.length;
    setMatchIndex(next);
    setPageNumber(matches.matches[next].page);
  };

  const prevMatch = () => {
    if (!matches?.matches.length) return;
    const prev =
      (matchIndex - 1 + matches.matches.length) % matches.matches.length;
    setMatchIndex(prev);
    setPageNumber(matches.matches[prev].page);
  };

  const handleItemClick = (args: OnItemClickArgs): void => {
    setPageNumber(args.pageNumber);
  };

  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if ((e.ctrlKey || e.metaKey) && e.key === "f") {
        e.preventDefault();
        setSearchOpen(true);
        setTimeout(() => {
          searchRef.current?.querySelector("input")?.focus();
        }, 0);
        // right arrow
      } else if (e.key === "ArrowRight" || e.key === "ArrowDown") {
        nextPage();
        // left arrow
      } else if (e.key === "ArrowLeft" || e.key === "ArrowUp") {
        prevPage();
      } else if ((e.ctrlKey || e.metaKey) && e.key === "+") {
        e.preventDefault();
        zoomIn();
      } else if ((e.ctrlKey || e.metaKey) && e.key === "-") {
        e.preventDefault();
        zoomOut();
      }
    };

    const handleClickOutside = (e: MouseEvent) => {
      const target = e.target as HTMLElement;
      if (searchRef.current && !searchRef.current.contains(target)) {
        setSearchOpen(false);
      }
    };

    window.addEventListener("click", handleClickOutside);
    window.addEventListener("keydown", handleKeyDown);
    return () => {
      window.removeEventListener("keydown", handleKeyDown);
      window.removeEventListener("click", handleClickOutside);
    };
  }, [nextPage, prevPage]);

  const textRenderer = useCallback(
    (textItem: { str: string }) =>
      highlightPattern(textItem.str, new RegExp(query, "gi"), highlightColor),
    [query, highlightColor]
  );

  // --- UI ---
  return (
    <div className="overflow-hidden flex flex-col h-full w-full">
      <div className="flex items-center justify-between w-full px-3 py-2 bg-gray-100 border-b shrink-0 gap-2 flex-wrap relative">
        <div
          className={cn(
            "absolute -bottom-4 transform translate-y-full right-4 z-999 bg-white border rounded-lg shadow-md p-3 items-center gap-2 flex",
            !searchOpen && "hidden"
          )}
          ref={searchRef}
        >
          <Input
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            placeholder="Search in document..."
            className="w-64"
            onKeyDown={(e) => e.key === "Enter" && handleSearch()}
          />
          <span className="text-sm text-gray-500">
            {matches && matches.matches.length > 0
              ? `${matchIndex + 1} of ${matches.matches.length}`
              : "0 results"}
          </span>
          <button
            onClick={prevMatch}
            className="px-1 text-gray-600 hover:text-black"
          >
            <ChevronLeft className="w-4 h-4" />
          </button>
          <button
            onClick={nextMatch}
            className="px-1 text-gray-600 hover:text-black"
          >
            <ChevronRight className="w-4 h-4" />
          </button>
        </div>

        {/* Zoom controls */}
        <div className="flex items-center gap-1">
          <Button
            onClick={zoomOut}
            size={"icon"}
            variant={"ghost"}
            className="p-2 rounded-lg hover:bg-gray-200"
            title="Zoom Out"
          >
            <ZoomOut className="w-4 h-4" />
          </Button>
          <Button
            onClick={resetZoom}
            size={"icon"}
            variant={"ghost"}
            disabled={scale === 1.0}
            className="p-2 rounded-lg hover:bg-gray-200"
            title="Reset Zoom"
          >
            <RefreshCw className="w-4 h-4" />
          </Button>
          <Button
            onClick={zoomIn}
            size={"icon"}
            variant={"ghost"}
            className="p-2 rounded-lg hover:bg-gray-200"
            title="Zoom In"
          >
            <ZoomIn className="w-4 h-4" />
          </Button>
        </div>

        {/* Pagination */}
        <div className="flex items-center gap-2">
          <Button
            onClick={prevPage}
            disabled={pageNumber <= 1}
            size={"icon"}
            variant={"ghost"}
            className="p-2 rounded-lg hover:bg-gray-200 disabled:opacity-40"
          >
            <ChevronLeft className="w-4 h-4" />
          </Button>

          <div>
            <Input
              type="number"
              min={1}
              max={numPages}
              value={requestedPageNumber}
              onChange={(e) => {
                let val = parseInt(e.target.value, 10);
                if (isNaN(val)) val = 1;
                setRequestedPageNumber(Math.min(Math.max(val, 1), numPages));
              }}
              className="p-0! w-auto text-center appearance-none"
            />
            <span className="text-sm font-medium text-gray-600">
              {" "}
              / {numPages || "—"}
            </span>
          </div>

          <Button
            onClick={nextPage}
            disabled={pageNumber >= numPages}
            size={"icon"}
            variant={"ghost"}
            className="p-2 rounded-lg hover:bg-gray-200 disabled:opacity-40"
          >
            <ChevronRight className="w-4 h-4" />
          </Button>
        </div>

        <div className="text-xs text-gray-500">
          Zoom: {(scale * 100).toFixed(0)}%
        </div>
      </div>

      {/* PDF area */}
      <div
        className="flex-1 overflow-auto flex justify-center bg-gray-50"
        ref={containerRef}
        tabIndex={0}
      >
        {loading && (
          <div className="flex flex-col items-center justify-center h-full text-gray-400">
            <Loader2 className="w-5 h-5 animate-spin mb-2" />
            Loading PDF...
          </div>
        )}

        <Document
          file={url}
          onLoadSuccess={handleDocumentLoad}
          loading=""
          onItemClick={handleItemClick}
          className={"flex justify-center items-center"}
        >
          <Page
            loading=""
            height={pageSize?.pageHeight || undefined}
            width={pageSize?.pageWidth || undefined}
            pageNumber={pageNumber}
            scale={scale}
            onLoadSuccess={setPage}
            renderAnnotationLayer
            renderTextLayer
            customTextRenderer={textRenderer}
          />
        </Document>
      </div>
    </div>
  );
}

export const pdfPreviewHandler: FilePreviewHandler = {
  type: "pdf",
  canHandle: (ext, mime) => ext === "pdf" || mime === "application/pdf",
  renderInline: ({ url, name }) => (
    <iframe src={url} title={name} className="w-full h-48 border rounded-md" />
  ),
  renderModal: ({ url, name }) => <PdfViewer url={url} name={name} />,
};
