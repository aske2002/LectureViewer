import ReactMarkdown from "react-markdown";
import remarkGfm from "remark-gfm";
import { Card } from "@/components/ui/card";
import { Textarea } from "@/components/ui/textarea";
import { Tabs, TabsList, TabsTrigger, TabsContent } from "@/components/ui/tabs";
import hljs from "highlight.js";
import "highlight.js/styles/github-dark.css";
import MarkdownEditor from "./markdown-editor";
import { MilkdownEditor } from "./milkdown-editor";

interface MarkdownInputProps {
  value: string;
  onChange: (value: string) => void;
}

export default function MarkdownInput({ value, onChange }: MarkdownInputProps) {
  return (
    <Card className="w-full p-0 overflow-hidden flex-flex-col min-h-32">
      <Tabs defaultValue="edit" className="w-full flex flex-col overflow-hidden">
        {/* Tabs header */}
        <TabsList className="grid grid-cols-3 w-full">
          <TabsTrigger value="edit">Editor</TabsTrigger>
          <TabsTrigger value="preview">Preview</TabsTrigger>
          <TabsTrigger value="split">Split View</TabsTrigger>
        </TabsList>

        <div className="overflow-auto">
          <TabsContent value="edit">
            <MilkdownEditor value={value} onChange={onChange} />
          </TabsContent>

          <TabsContent value="preview">
            <MarkdownPreview content={value} />
          </TabsContent>

          <TabsContent value="split">
            <div className="grid grid-cols-2 gap-4">
              <MilkdownEditor value={value} onChange={onChange} />
              <MarkdownPreview content={value} />
            </div>
          </TabsContent>
        </div>
      </Tabs>
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
