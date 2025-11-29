import { useEffect, useRef, useState } from "react";
import { useDebouncedCallback } from "use-debounce";
import { defaultExtensions } from "./extensions";
import type { JSONContent, Editor as EditorInstance } from "@tiptap/react";
import { slashCommand } from "./slash-command";

import "@/style/prosemirror.css";
import "katex/dist/katex.min.css";
import { cn } from "@/lib/utils";
import ProseView, { ProseViewFontSize } from "./prose-view";

interface EditorProps {
  initialContent?: JSONContent;
  editorKey?: string;
  className?: string;
  onChange?: (content: JSONContent) => void;
}

export const Editor = ({
  initialContent: externalInitialContent,
  onChange,
  editorKey,
  className,
}: EditorProps) => {
  const [initialContent, setInitialContent] = useState<null | JSONContent>(
    null
  );
  const [saveStatus, setSaveStatus] = useState("Saved");
  const [charsCount, setCharsCount] = useState<number | null>(null);

  useEffect(() => {
    onChange?.(initialContent!);
  }, [initialContent]);

  const debouncedUpdates = useDebouncedCallback(
    async (editor: EditorInstance) => {
      const json = editor.getJSON();
      onChange?.(json);
      setCharsCount(editor.storage.characterCount.words());
      if (editorKey) {
        window.localStorage.setItem(
          `novel-content-${editorKey}`,
          JSON.stringify(json)
        );
      }
      setSaveStatus("Saved");
    },
    500
  );

  useEffect(() => {
    if (externalInitialContent) {
      setInitialContent(externalInitialContent);
      return;
    } else if (editorKey) {
      const content = window.localStorage.getItem(`novel-content-${editorKey}`);
      if (content) {
        setInitialContent(JSON.parse(content));
        return;
      }
    }

    setInitialContent({
      type: "doc",
      content: [],
    });
  }, []);

  if (!initialContent) return null;

  return (
    <div
      className={cn(
        "flex w-full max-w-5xl overflow-hidden relative",
        className
      )}
    >
      <div className="flex absolute right-5 top-5 z-10 mb-5 gap-2">
        <div className="rounded-lg bg-accent px-2 py-1 text-sm text-muted-foreground">
          {saveStatus}
        </div>
        <div
          className={
            charsCount
              ? "rounded-lg bg-accent px-2 py-1 text-sm text-muted-foreground"
              : "hidden"
          }
        >
          {charsCount} Words
        </div>
      </div>
      <ProseView
        onUpdate={({ editor }) => debouncedUpdates(editor)}
        content={initialContent}
      />
    </div>
  );
};

interface EditorPreviewProps {
  content: JSONContent;
  className?: string;
  maxHeight?: number;
  fontSize?: ProseViewFontSize;
  allowToggle?: boolean;
  muted?: boolean;
}

export const EditorPreview = ({
  content,
  maxHeight = 180,
  className,
  fontSize,
  muted,
  allowToggle = true,
}: EditorPreviewProps) => {
  const containerRef = useRef<HTMLDivElement>(null);
  const [isOverflowing, setIsOverflowing] = useState(false);
  const [expanded, setExpanded] = useState(false);

  useEffect(() => {
    const el = containerRef.current;
    if (el) {
      setIsOverflowing(el.scrollHeight > maxHeight);
    }
  }, [content, maxHeight]);

  return (
    <div className="relative">
      <div
        ref={containerRef}
        className={className}
        style={{
          maxHeight: expanded ? "none" : maxHeight,
          overflow: "hidden",
          position: "relative",
        }}
      >
        <ProseView content={content} className={cn(className, { "opacity-80": muted })} editable={false} fontSize={fontSize} />
        {/* Fade-out shadow at bottom */}
        {!expanded && isOverflowing && (
          <div className="pointer-events-none absolute bottom-0 left-0 right-0 h-16 bg-linear-to-t from-background to-transparent" />
        )}
      </div>
      {/* Toggle button */}
      {isOverflowing && allowToggle && (
        <button
          onClick={(e) => {
            e.preventDefault();
            e.stopPropagation();
            setExpanded(!expanded);
          }}
          className="mt-2 text-sm text-primary hover:underline"
        >
          {expanded ? "Show less" : "Show more"}
        </button>
      )}
    </div>
  );
};
