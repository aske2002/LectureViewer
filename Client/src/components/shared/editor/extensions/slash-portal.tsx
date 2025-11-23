import { createContext, useEffect, useState } from "react";
import { EditorCommandOut, EditorCommandOutProps } from "../editor-command";
import { EditorEvents, useCurrentEditor } from "@tiptap/react";
import { SlashCommandPluginKey } from "./slash-command";

export const SlashPortalContext = createContext<HTMLElement | null>(null);
export let updateSlashPortal: (props: any) => void = () => {};

export const SlashPortalRenderer = () => {
  const [portalProps, setPortalProps] = useState<EditorCommandOutProps | null>(
    null
  );
  const { editor } = useCurrentEditor();

  useEffect(() => {
    updateSlashPortal = setPortalProps;
  }, []);

  useEffect(() => {
    if (!editor) return;

    const handleTransaction = ({ editor }: EditorEvents["transaction"]) => {
      const pluginState = SlashCommandPluginKey.getState(editor.state);
      console.log("SlashPortalRenderer transaction", { pluginState });
    };

    editor.on("transaction", handleTransaction);

    return () => {
      editor.off("transaction", handleTransaction);
    };
  }, [editor]);

  if (!portalProps) return null;

  return (
    <EditorCommandOut
      {...portalProps}
      onClose={() => setPortalProps({ ...portalProps, open: false })}
    />
  );
};
