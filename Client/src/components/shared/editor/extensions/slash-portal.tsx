import { useEffect, useRef, useState } from "react";
import { EditorCommandOut } from "../editor-command";
import { EditorEvents, useCurrentEditor } from "@tiptap/react";
import { SlashCommandOptions, SlashCommandPluginKey } from "./slash-command";

export const SlashPortalRenderer = () => {
  const [portalProps, setPortalProps] = useState<SlashCommandOptions | null>(
    null
  );
  const { editor } = useCurrentEditor();

  useEffect(() => {
    if (!editor) return;

    const handleTransaction = ({ editor }: EditorEvents["transaction"]) => {
      const pluginState = SlashCommandPluginKey.getState(editor.state);
      setPortalProps(pluginState ? pluginState : null);
    };

    editor.on("transaction", handleTransaction);

    return () => {
      editor.off("transaction", handleTransaction);
    };
  }, [editor]);

  const handleClose = () => {
    if (!editor) return;
    console.log("Closing slash command portal");
    editor.view.dispatch(
      editor.state.tr.setMeta(SlashCommandPluginKey, {
        ...portalProps,
        open: false,
      })
    );
  };

  if (!portalProps) return null;

  return (
    <EditorCommandOut
      {...portalProps}
      onClose={handleClose}
    />
  );
};
