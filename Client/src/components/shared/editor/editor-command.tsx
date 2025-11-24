import { useAtom, useSetAtom } from "jotai";
import { useEffect, forwardRef } from "react";
import { Command, CommandInput, CommandList } from "@/components/ui/command";
import { queryAtom, rangeAtom } from "./utils/atoms";
import { novelStore } from "./utils/store";
import type { ComponentPropsWithoutRef, FC } from "react";
import { Popover, PopoverContent } from "@/components/ui/popover";
import { PopoverAnchor } from "@radix-ui/react-popover";
import { SlashCommandOptions } from "./extensions/slash-command";
import { EditorCommandTunnelContext } from "./utils/commandTunnel";

export const EditorCommandOut: FC<
  SlashCommandOptions & {
    onClose: () => void;
  }
> = ({ query, range, onClose, clientRect, open = false }) => {
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

  if (!open) return null;

  return (
    <EditorCommandTunnelContext.Consumer>
      {(tunnelInstance) => (
        <Popover
          open={open}
          modal={true}
          onOpenChange={(open) => !open && onClose && onClose()}
        >
          <PopoverAnchor
            virtualRef={{
              current: {
                getBoundingClientRect: () => {
                  return clientRect?.() || new DOMRect(0, 0, 0, 0);
                },
              },
            }}
          />

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
