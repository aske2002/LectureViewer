import { Extension } from "@tiptap/core";
import type { Editor, Range } from "@tiptap/core";
import Suggestion, {
  SuggestionKeyDownProps,
  SuggestionOptions,
  SuggestionProps,
} from "@tiptap/suggestion";
import { PluginKey } from "@tiptap/pm/state";

import { type RefObject } from "react";
import type { ReactNode } from "react";
import { updateSlashPortal } from "./slash-portal";
import { ReactRenderer } from "@tiptap/react";

export const SlashCommandPluginKey = new PluginKey<SlashCommandOptions>(
  "slashCommand"
);

export type SlashCommandOptions = {
  clientRect?: DOMRect | null;
  editor: Editor;
  query: string;
  range: Range;
  element: RefObject<HTMLElement> | null;
  open?: boolean;
};

const Command = Extension.create<
  Partial<SuggestionOptions>,
  SlashCommandOptions
>({
  name: "slash-command",
  addOptions() {
    return {
      ...this.parent?.(),
      char: "/",
      command: ({
        editor,
        range,
        props,
      }: {
        editor: Editor;
        range: Range;
        props: SuggestionProps;
      }) => {
        console.log("SlashCommand command executed", { props });
        props.command({ editor, range });
      },
    };
  },
  addProseMirrorPlugins() {
    console.log("Options", this.options);
    return [
      Suggestion({
        pluginKey: SlashCommandPluginKey,
        
        ...this.options,
        editor: this.editor,
      }),
    ];
  },
});

const renderItems = (elementRef?: RefObject<Element> | null) => {
  return {
    onStart: (props: SuggestionProps) => {
      const { editor } = props;

      editor.view.dispatch(
        editor.view.state.tr.setMeta(SlashCommandPluginKey, {
          open: true,
          element: elementRef,
          query: props.query,
          range: props.range,
          clientRect: props.clientRect?.(),
        })
      );
    },

    onUpdate: (props: SuggestionProps) => {
      const { editor } = props;

      editor.view.dispatch(
        editor.view.state.tr.setMeta(SlashCommandPluginKey, {
          open: true,
          element: elementRef,
          query: props.query,
          range: props.range,
          clientRect: props.clientRect?.(),
        })
      );
    },

    onKeyDown: ({ event, view }: SuggestionKeyDownProps) => {
      if (event.key === "Escape") {
        const current = SlashCommandPluginKey.getState(view.state);

        view.dispatch(
          view.state.tr.setMeta(SlashCommandPluginKey, {
            ...current,
            open: false,
          })
        );
        return true;
      }
      return false;
    },

    onExit: () => {
      updateSlashPortal({ open: false });
    },
  };
};

export interface SuggestionItem {
  title: string;
  description: string;
  icon: ReactNode;
  searchTerms?: string[];
  command?: (props: { editor: Editor; range: Range }) => void;
}

export const createSuggestionItems = (items: SuggestionItem[]) => items;

export const handleCommandNavigation = (event: KeyboardEvent) => {
  if (["ArrowUp", "ArrowDown", "Enter"].includes(event.key)) {
    const slashCommand = document.querySelector("#slash-command");
    if (slashCommand) {
      return true;
    }
  }
};

export { Command, renderItems };
