import { EditorProvider } from "@tiptap/react";
import type { EditorProviderProps, JSONContent } from "@tiptap/react";
import { Provider } from "jotai";
import { forwardRef, useRef } from "react";
import type { FC, ReactNode } from "react";
import tunnel from "tunnel-rat";
import { novelStore } from "./utils/store";
import { MathBubblePortal } from "./extensions/math-bubble";
import { EditorCommandTunnelContext } from "./utils/commandTunnel";

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
        {children}
      </EditorProvider>
    </div>
  )
);

EditorContent.displayName = "EditorContent";
