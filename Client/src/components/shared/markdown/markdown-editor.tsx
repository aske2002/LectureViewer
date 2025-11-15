import { Button } from "@/components/ui/button";
import { Textarea } from "@/components/ui/textarea";
import { Card } from "@/components/ui/card";
import {
  Bold,
  Italic,
  Heading1,
  Heading2,
  Link,
  Heading3,
  Link as LinkIcon,
  List,
  ListOrdered,
  Sigma,
  Code,
  Code2,
} from "lucide-react";
import { cn } from "@/lib/utils";
import { useRef } from "react";
import "@/style/markdown.css"

interface MarkdownEditorProps {
  value: string;
  onChange: (value: string) => void;
  className?: string;
}

export default function MarkdownEditor({
  value,
  onChange,
  className,
}: MarkdownEditorProps) {
  const textareaRef = useRef<HTMLTextAreaElement>(null);

  /** Insert text with a selected range */
  const insert = (before: string, placeholder: string, after: string = "") => {
    const textarea = textareaRef.current;
    if (!textarea) return;

    const start = textarea.selectionStart;
    const end = textarea.selectionEnd;

    const newText =
      value.slice(0, start) + before + placeholder + after + value.slice(end);

    onChange(newText);

    // Move cursor + select placeholder
    setTimeout(() => {
      textarea.focus();
      textarea.setSelectionRange(
        start + before.length,
        start + before.length + placeholder.length
      );
    });
  };

  return (
    <Card className="p-4 max-w-3xl mx-auto space-y-3">
      {/* Toolbar */}
      <div className="flex flex-wrap gap-2">
        <Button
          variant="outline"
          size="sm"
          onClick={() => insert("**", "bold text", "**")}
        >
          <Bold className="w-4 h-4" />
        </Button>

        <Button
          variant="outline"
          size="sm"
          onClick={() => insert("*", "italic text", "*")}
        >
          <Italic className="w-4 h-4" />
        </Button>

        <Button
          variant="outline"
          size="sm"
          onClick={() => insert("# ", "Heading 1")}
        >
          <Heading1 className="w-4 h-4" />
        </Button>

        <Button
          variant="outline"
          size="sm"
          onClick={() => insert("## ", "Heading 2")}
        >
          <Heading2 className="w-4 h-4" />
        </Button>

        <Button
          variant="outline"
          size="sm"
          onClick={() => insert("### ", "Heading 3")}
        >
          <Heading3 className="w-4 h-4" />
        </Button>

        <Button
          variant="outline"
          size="sm"
          onClick={() => insert("- ", "List item")}
        >
          <List className="w-4 h-4" />
        </Button>

        <Button
          variant="outline"
          size="sm"
          onClick={() => insert("1. ", "List item")}
        >
          <ListOrdered className="w-4 h-4" />
        </Button>

        <Button
          variant="outline"
          size="sm"
          onClick={() => insert("[", "text", "](url)")}
        >
          <LinkIcon className="w-4 h-4" />
        </Button>

        <Button
          variant="outline"
          size="sm"
          onClick={() => insert("`", "code", "`")}
        >
          <Code className="w-4 h-4" />
        </Button>

        <Button
          variant="outline"
          size="sm"
          onClick={() => insert("\n```\n", "code block", "\n```\n")}
        >
          <Code2 className="w-4 h-4" />
        </Button>

        <Button
          variant="outline"
          size="sm"
          onClick={() => insert("$", "latex", "$")}
        >
          <Sigma className="w-4 h-4" />
        </Button>

        <Button
          variant="outline"
          size="sm"
          onClick={() => insert("\n$$\n", "latex block", "\n$$\n")}
        >
          <Sigma className="w-4 h-4" />
        </Button>
      </div>

      {/* Editor */}
      <Textarea
        ref={textareaRef}
        className="font-mono text-sm h-[300px]"
        value={value}
        onChange={(e) => onChange(e.target.value)}
      />
    </Card>
  );
}
