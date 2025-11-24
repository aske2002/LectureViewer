import { Plugin, PluginKey, TextSelection } from "@tiptap/pm/state";
import { EditorEvents, Extension } from "@tiptap/core";
import type { Node } from "@tiptap/pm/model";

import { useEffect, useState, useRef, createRef } from "react";
import { Input } from "@/components/ui/input"; // shadcn
import { useCurrentEditor } from "@tiptap/react";
import { Popover, PopoverContent } from "@/components/ui/popover";
import { PopoverAnchor } from "@radix-ui/react-popover";
interface MathBubbleUpdatePayload {
  node: null | Node;
  pos: number | null;
  size: number | null;
}

interface MathBubbleStorage {
  data: MathBubbleUpdatePayload;
}

declare module "@tiptap/core" {
  interface Storage {
    mathBubble: MathBubbleStorage;
  }
}

export function MathBubblePortal() {
  const { editor } = useCurrentEditor();

  const inputRef = createRef<HTMLInputElement>();
  const elementRef = useRef<HTMLElement>(null);
  const measureRef = useRef<{
    getBoundingClientRect(): DOMRect;
  }>({
    getBoundingClientRect: () => {
      const rect = elementRef.current?.getBoundingClientRect() || new DOMRect();
      return rect;
    },
  });

  const [state, setState] = useState<MathBubbleUpdatePayload>({
    node: null,
    pos: editor?.state.selection.from || null,
    size: null,
  });

  // subscribe ONLY to this plugin’s state
  useEffect(() => {
    if (!editor) return;

    const handleTransaction = ({ editor }: EditorEvents["transaction"]) => {
      const pluginState = MathBubblePluginKey.getState(editor.state);
      setState(pluginState || { node: null, pos: null, size: null });
      elementRef.current =
        pluginState?.pos != null && editor
          ? (editor.view.nodeDOM(pluginState.pos) as HTMLElement)
          : null;
    };

    editor.on("transaction", handleTransaction);

    return () => {
      editor.off("transaction", handleTransaction);
    };
  }, [editor]);

  useEffect(() => {
    if (state.node && inputRef.current) {
      inputRef.current.focus();
    }
  }, [state]);

  const open = state.node != null;

  const save = (value: string) => {
    if (!editor || state.pos == null || state.node == null) return;
    editor.commands.updateMath(state.pos, value);
  };

  const close = (direction: "left" | "right" = "right") => {
    if (!editor) return;
    const state = MathBubblePluginKey.getState(editor?.state);
    if (!state || state.pos == null || state.node == null) return;

    const exitTarget =
      direction === "left"
        ? state.pos - 1
        : state.pos + state.node.nodeSize + 1;

    if (!elementRef.current) return null;

    editor.view.dispatch(
      editor.state.tr.setSelection(
        TextSelection.create(editor.state.doc, exitTarget)
      )
    );

    editor.view.dispatch(
      editor.state.tr.setMeta(MathBubblePluginKey, {
        close: true,
      })
    );

    editor.commands.focus();
  };

  return (
    <Popover
      modal={true}
      open={open}
      onOpenChange={(o) => {
        if (!o) {
          close();
        }
      }}
    >
      <PopoverAnchor virtualRef={measureRef} />
      <PopoverContent
        side="bottom"
        alignOffset={0}
        sideOffset={0}
        align="start"
        className="p-0"
        onOpenAutoFocus={(e) => e.preventDefault()}
      >
        <Input
          ref={inputRef}
          defaultValue={state.node?.attrs.latex}
          autoFocus={true}
          onChange={(e) => {
            save(e.currentTarget.value);
          }}
          onKeyDown={(e) => {
            if (
              e.key === "Enter" ||
              e.key === "Escape" ||
              (e.key === "ArrowRight" &&
                e.currentTarget.selectionEnd ===
                  e.currentTarget.value.length) ||
              e.key === "ArrowDown"
            ) {
              e.preventDefault();
              close("right");
            }

            if (
              (e.key === "ArrowLeft" && e.currentTarget.selectionStart === 0) ||
              e.key === "ArrowUp"
            ) {
              e.preventDefault();
              close("left");
            }
          }}
        />
      </PopoverContent>
    </Popover>
  );
}

export const MathBubblePluginKey = new PluginKey<MathBubbleUpdatePayload>(
  "mathBubble"
);

export const MathBubble = Extension.create<{}, MathBubbleStorage>({
  name: "mathBubble",

  addStorage() {
    return {
      data: { node: null, pos: null, size: null },
    };
  },

  addProseMirrorPlugins() {
    const editor = this.editor;

    return [
      new Plugin({
        key: MathBubblePluginKey,

        state: {
          init(): MathBubbleUpdatePayload {
            return { node: null, pos: null, size: null };
          },

          apply(tr, prev, oldState, newState): MathBubbleUpdatePayload {
            // read a possible explicit close from meta
            const meta = tr.getMeta(MathBubblePluginKey);
            if (meta && meta.close) {
              editor.storage.mathBubble.data = {
                node: null,
                pos: null,
                size: null,
              };
              return { node: null, pos: null, size: null };
            }

            const sel = newState.selection;

            const node =
              sel.$from.nodeAfter?.type.name === "math"
                ? sel.$from.nodeAfter
                : sel.$from.nodeBefore?.type.name === "math"
                  ? sel.$from.nodeBefore
                  : null;

            if (!node) {
              editor.storage.mathBubble.data = {
                node: null,
                pos: null,
                size: null,
              };
              return { node: null, pos: null, size: null };
            }

            // get correct pos for inline atom
            const pos =
              sel.$from.nodeAfter === node
                ? sel.$from.pos
                : sel.$from.pos - node.nodeSize;

            editor.storage.mathBubble.data = { node, pos, size: node.nodeSize };
            return { node, pos, size: node.nodeSize };
          },
        },
      }),
    ];
  },
});
