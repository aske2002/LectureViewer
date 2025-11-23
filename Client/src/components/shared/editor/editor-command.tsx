import { useAtom, useSetAtom } from "jotai";
import { useEffect, forwardRef, createContext } from "react";
import { Command, CommandInput, CommandList } from "@/components/ui/command";
import { queryAtom, rangeAtom } from "./utils/atoms";
import { novelStore } from "./utils/store";
import type tunnel from "tunnel-rat";
import type { ComponentPropsWithoutRef, FC, RefObject } from "react";
import type { Range } from "@tiptap/core";
import { Popover, PopoverContent } from "@/components/ui/popover";
import { PopoverAnchor } from "@radix-ui/react-popover";

export const EditorCommandTunnelContext = createContext(
  {} as ReturnType<typeof tunnel>
);

export interface EditorCommandOutProps {
  readonly query: string;
  readonly range: Range;
  readonly element: RefObject<HTMLElement>;
  onClose?: () => void;
  open?: boolean;
}

export const EditorCommandOut: FC<EditorCommandOutProps> = ({
  query,
  range,
  onClose,
  element,
  open = false,
}) => {
  const setQuery = useSetAtom(queryAtom, { store: novelStore });
  const setRange = useSetAtom(rangeAtom, { store: novelStore });

  useEffect(() => {
    setQuery(query);
  }, [query, setQuery]);

  useEffect(() => {
    setRange(range);
  }, [range, setRange]);

  useEffect(() => {
    const navigationKeys = ["ArrowUp", "ArrowDown", "Enter"];
    const onKeyDown = (e: KeyboardEvent) => {
      if (navigationKeys.includes(e.key)) {
        e.preventDefault();
        const commandRef = document.querySelector("#slash-command");

        if (commandRef)
          commandRef.dispatchEvent(
            new KeyboardEvent("keydown", {
              key: e.key,
              cancelable: true,
              bubbles: true,
            })
          );

        return false;
      }
    };
    document.addEventListener("keydown", onKeyDown);
    return () => {
      document.removeEventListener("keydown", onKeyDown);
    };
  }, []);

  console.log("EditorCommandOut render", { open, query, range });

  return (
    <EditorCommandTunnelContext.Consumer>
      {(tunnelInstance) => (
        <Popover
          open={open}
          modal={true}
          onOpenChange={(open) => !open && onClose && onClose()}
        >
          <PopoverAnchor virtualRef={element}>
            <PopoverContent
              side="bottom"
              alignOffset={0}
              sideOffset={0}
              align="start"
              className="p-0"
              onOpenAutoFocus={(e) => e.preventDefault()}
            >
              <tunnelInstance.Out />
            </PopoverContent>
          </PopoverAnchor>
        </Popover>
      )}
    </EditorCommandTunnelContext.Consumer>
  );
};

export const EditorCommand = forwardRef<
  HTMLDivElement,
  ComponentPropsWithoutRef<typeof Command>
>(({ children, className, ...rest }, ref) => {
  const [query, setQuery] = useAtom(queryAtom);

  return (
    <EditorCommandTunnelContext.Consumer>
      {(tunnelInstance) => (
        <tunnelInstance.In>
          <Command
            ref={ref}
            onKeyDown={(e) => {
              e.stopPropagation();
            }}
            id="slash-command"
            className={className}
            {...rest}
          >
            <div className="hidden">
              <CommandInput value={query} />
            </div>

            {children}
          </Command>
        </tunnelInstance.In>
      )}
    </EditorCommandTunnelContext.Consumer>
  );
});
export const EditorCommandList = CommandList;

EditorCommand.displayName = "EditorCommand";
