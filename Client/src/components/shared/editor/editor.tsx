import { EditorProvider } from "@tiptap/react";
import type { EditorProviderProps, JSONContent } from "@tiptap/react";
import { Provider } from "jotai";
import { forwardRef, useRef } from "react";
import type { FC, ReactNode } from "react";
import tunnel from "tunnel-rat";
import { novelStore } from "./utils/store";
import { EditorCommandTunnelContext } from "./editor-command";
import { SlashPortalRenderer } from "./extensions/slash-portal";
import { MathBubblePortal } from "./extensions/math-bubble";

export interface EditorProps {
  readonly className?: string;
  initialContent?: JSONContent;
  onChange?: (content: { markdown: string; json: JSONContent }) => void;
}

export const Editor: FC<EditorProps> = ({
  className,
  initialContent,
  onChange,
}) => {
  return (
    <EditorRoot>
      <EditorContent
        className={className}
        initialContent={initialContent}
        onUpdate={({ editor }) => {
          if (onChange) {
            onChange({
              markdown: editor.getMarkdown(),
              json: editor.getJSON(),
            });
          }
        }}
      ></EditorContent>
    </EditorRoot>
  );
};

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

export type EditorContentProps = Omit<EditorProviderProps, "content"> & {
  readonly children?: ReactNode;
  readonly className?: string;
  readonly initialContent?: JSONContent;
};

export const EditorContent = forwardRef<HTMLDivElement, EditorContentProps>(
  ({ className, children, initialContent, ...rest }, ref) => (
    <div ref={ref} className={className}>
      <EditorProvider {...rest} content={initialContent}>
        <MathBubblePortal />
        <SlashPortalRenderer />
        {children}
      </EditorProvider>
    </div>
  )
);

EditorContent.displayName = "EditorContent";
