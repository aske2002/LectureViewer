import { Extension } from "@tiptap/core";
import type { Editor, Range } from "@tiptap/core";
import Suggestion, {
  SuggestionKeyDownProps,
  SuggestionOptions,
  SuggestionProps,
} from "@tiptap/suggestion";
import { PluginKey, Plugin } from "@tiptap/pm/state";
import { type RefObject } from "react";
import type { ReactNode } from "react";

export const SlashCommandSuggestionPluginKey = new PluginKey<SuggestionOptions>(
  "slashCommandSuggestion"
);
export const SlashCommandPluginKey = new PluginKey<SlashCommandOptions>(
  "slashCommand"
);

export type SlashCommandOptions = {
  clientRect?: (() => DOMRect | null) | null;
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
        props.command({ editor, range });
      },
    };
  },
  addProseMirrorPlugins() {
    const suggestionPlugin = Suggestion({
      pluginKey: SlashCommandSuggestionPluginKey,
      ...this.options,
      editor: this.editor,
    });
    return [
      new Plugin<SlashCommandOptions>({
        key: SlashCommandPluginKey,
        state: {
          init: () => ({
            open: false,
            editor: this.editor,
            element: null,
            query: "",
            range: {
              from: 0,
              to: 0,
            },
            clientRect: () => null,
          }),
          apply(tr, prev) {
            const meta = tr.getMeta(SlashCommandPluginKey);
            if (!meta) return prev;
            return {
              ...prev,
              ...meta, // merge new values
            };
          },
        },
        props: {}, // optional
      }),

      suggestionPlugin,
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
          clientRect: props.clientRect,
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
          clientRect: props.clientRect,
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

    onExit: (props: SuggestionProps) => {
      const { editor } = props;

      editor.view.dispatch(
        editor.view.state.tr.setMeta(SlashCommandPluginKey, {
          ...SlashCommandPluginKey.getState(editor.view.state),
          open: false,
        })
      );
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
