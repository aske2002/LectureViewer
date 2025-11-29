import { FC, forwardRef, ReactNode, useMemo, useRef, useState } from "react";
import { defaultExtensions } from "./extensions";
import { ColorSelector } from "./selectors/color-selector";
import { LinkSelector } from "./selectors/link-selector";
import { MathSelector } from "./selectors/math-selector";
import { NodeSelector } from "./selectors/node-selector";
import {
  type JSONContent,
  EditorProvider,
  EditorProviderProps,
} from "@tiptap/react";
import { uploadFn } from "./image-upload";
import { TextButtons } from "./selectors/text-buttons";
import { slashCommand, suggestionItems } from "./slash-command";
import EditorCommandItem, { EditorCommandEmpty } from "./editor-command-item";
import { EditorCommand, EditorCommandList } from "./editor-command";
import { handleImageDrop, handleImagePaste } from "./plugins";
import { handleCommandNavigation } from "./extensions/slash-command";
import { ImageResizer } from "./extensions/image-resizer";
import { Separator } from "@/components/ui/separator";
import GenerativeMenuSwitch from "./generative/generative-menu-switch";

import "@/style/prosemirror.css";
import "katex/dist/katex.min.css";
import { SlashPortalRenderer } from "./extensions/slash-portal";
import { cn } from "@/lib/utils";
import tunnel from "tunnel-rat";
import { Provider } from "jotai";
import { EditorCommandTunnelContext } from "./utils/commandTunnel";
import { novelStore } from "./utils/store";
import { MathBubblePortal } from "./extensions/math-bubble";
import { EditorState } from "prosemirror-state";

const extensions = [...defaultExtensions, slashCommand];

export type ProseViewFontSize = "sm" | "md" | "lg" | "xl";

type ProseViewProps = Omit<EditorContentProps, "extensions" | "slotAfter"> & {
  fontSize?: ProseViewFontSize;
};

const ProseView = ({
  className,
  editorProps,
  fontSize = "lg",
  ...rest
}: ProseViewProps) => {
  const [openNode, setOpenNode] = useState(false);
  const [openColor, setOpenColor] = useState(false);
  const [openLink, setOpenLink] = useState(false);
  const [openAI, setOpenAI] = useState(false);

  const attributes = useMemo(() => {
    const attributes = editorProps?.attributes;
    const className = cn(
      "prose dark:prose-invert prose-headings:font-title font-default focus:outline-none max-w-full",
      {
        "prose-sm": fontSize === "sm",
        "prose-md": fontSize === "md",
        "prose-lg": fontSize === "lg",
        "prose-xl": fontSize === "xl",
      },
      ""
    );
    if (typeof attributes === "function") {
      return (editor: EditorState) => {
        const propAttributes = attributes(editor) || {};
        return {
          ...propAttributes,
          class: cn(className, propAttributes.class),
        };
      };
    } else if (typeof attributes === "object") {
      return {
        ...attributes,
        class: cn(className, attributes.class),
      };
    }
    return { class: className };
  }, [editorProps?.attributes]);

  return (
    <EditorRoot>
      <EditorContent
        {...rest}
        extensions={extensions}
        className={cn("relative w-full max-w-5xl overflow-auto", className)}
        editorProps={{
          ...editorProps,
          handleDOMEvents: {
            keydown: (_view, event) => handleCommandNavigation(event),
          },
          handlePaste: (view, event) => handleImagePaste(view, event, uploadFn),
          handleDrop: (view, event, _slice, moved) =>
            handleImageDrop(view, event, moved, uploadFn),
          attributes,
        }}
        slotAfter={<ImageResizer />}
      >
        <EditorCommand className="h-auto max-h-[330px] overflow-y-auto rounded-md border border-muted bg-background px-1 py-2 shadow-sm transition-all">
          <EditorCommandEmpty className="px-2 text-muted-foreground">
            No results
          </EditorCommandEmpty>
          <EditorCommandList>
            {suggestionItems.map((item) => (
              <EditorCommandItem
                value={item.title}
                onCommand={(val) => item.command?.(val)}
                className="flex w-full items-center space-x-2 rounded-md px-2 py-1 text-left text-sm hover:bg-accent aria-selected:bg-accent"
                key={item.title}
              >
                <div className="flex h-10 w-10 items-center justify-center rounded-md border border-muted bg-background">
                  {item.icon}
                </div>
                <div>
                  <p className="font-medium">{item.title}</p>
                  <p className="text-xs text-muted-foreground">
                    {item.description}
                  </p>
                </div>
              </EditorCommandItem>
            ))}
          </EditorCommandList>
        </EditorCommand>
        <SlashPortalRenderer />

        <GenerativeMenuSwitch open={openAI} onOpenChange={setOpenAI}>
          <Separator orientation="vertical" />
          <NodeSelector open={openNode} onOpenChange={setOpenNode} />
          <Separator orientation="vertical" />

          <LinkSelector open={openLink} onOpenChange={setOpenLink} />
          <Separator orientation="vertical" />
          <MathSelector />
          <Separator orientation="vertical" />
          <TextButtons />
          <Separator orientation="vertical" />
          <ColorSelector open={openColor} onOpenChange={setOpenColor} />
        </GenerativeMenuSwitch>
      </EditorContent>
    </EditorRoot>
  );
};

export interface EditorProps {
  readonly className?: string;
  initialContent?: JSONContent;
  onChange?: (content: { markdown: string; json: JSONContent }) => void;
}

interface EditorRootProps {
  readonly children: ReactNode;
}

export const EditorRoot: FC<EditorRootProps> = ({ children }) => {
  const tunnelInstance = useRef(tunnel()).current;

  return (
    <Provider store={novelStore}>
      <EditorCommandTunnelContext.Provider value={tunnelInstance}>
        {children}
      </EditorCommandTunnelContext.Provider>
    </Provider>
  );
};

export type EditorContentProps = EditorProviderProps & {
  readonly children?: ReactNode;
  readonly className?: string;
};

export const EditorContent = forwardRef<HTMLDivElement, EditorContentProps>(
  ({ className, children, ...rest }, ref) => (
    <div ref={ref} className={className}>
      <EditorProvider {...rest}>
        <MathBubblePortal />
        {children}
      </EditorProvider>
    </div>
  )
);

EditorContent.displayName = "EditorContent";

export default ProseView;
