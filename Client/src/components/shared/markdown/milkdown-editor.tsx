import { useEffect, useLayoutEffect, useRef, useState } from "react";
import { replaceAll } from "@milkdown/kit/utils";
import { Crepe } from "@milkdown/crepe";
import { Milkdown } from "@milkdown/react";
import { Editor } from "@milkdown/core";

import "@milkdown/crepe/theme/common/style.css";
import "@milkdown/crepe/theme/nord.css";
import "@/style/markdown.css";


interface MilkdownEditorProps {
  onChange?: (value: string) => void;
  value?: string;
}

export function MilkdownEditor({ onChange, value }: MilkdownEditorProps) {
  const divRef = useRef<HTMLDivElement>(null);
  const [_value, _setValue] = useState(value || "");
  const [editor, setEditor] = useState<Editor | null>(null);

  useLayoutEffect(() => {
    if (!divRef.current) {
      return;
    }

    const crepe = new Crepe({
      root: divRef.current,
      defaultValue: _value,
    });

    crepe.on((listener) => {
      listener.markdownUpdated((_, markdown) => {
        _setValue(markdown);
      });
    });

    crepe.create().then((e) => {
      setEditor(e);
    });

    return () => {
      crepe.destroy();
      setEditor(null);
    };
  }, []);

  useEffect(() => {
    if (onChange) {
      onChange(_value);
    }
  }, [_value, onChange, editor]);

  useEffect(() => {
    if (value !== undefined && value !== _value) {
      editor?.action(replaceAll(value));
    }
  }, [value]);

  return (
    <div
      ref={divRef}
    />
  );

  return <Milkdown />;
}
