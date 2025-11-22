import ReactMarkdown from "react-markdown";
import remarkGfm from "remark-gfm";
import { Card } from "@/components/ui/card";
import hljs from "highlight.js";
import "highlight.js/styles/github-dark.css";
import { MilkdownEditor } from "./milkdown-editor";

interface MarkdownInputProps {
  value: string;
  onChange: (value: string) => void;
}

export default function MarkdownInput({ value, onChange }: MarkdownInputProps) {
  return (
    <Card className="w-full p-0 overflow-hidden flex-flex-col min-h-32 grow">
      <MilkdownEditor value={value} onChange={onChange} />
    </Card>
  );
}

interface MarkdownPreviewProps {
  content: string;
}

const MarkdownPreview = ({ content }: MarkdownPreviewProps) => {
  return (
    <div className="prose max-w-none p-2 overflow-auto bg-card rounded-md">
      <ReactMarkdown
        remarkPlugins={[remarkGfm]}
        components={{
          code({ className, children, ...props }) {
            const match = /language-(\w+)/.exec(className || "");
            return match ? (
              <pre>
                <code
                  className={className}
                  dangerouslySetInnerHTML={{
                    __html: hljs.highlight(match[1], {
                      language: String(children),
                    }).value,
                  }}
                />
              </pre>
            ) : (
              <code className={className} {...props}>
                {children}
              </code>
            );
          },
        }}
      >
        {content}
      </ReactMarkdown>
    </div>
  );
};
