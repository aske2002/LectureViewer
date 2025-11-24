import { Node as ProseNode } from "@tiptap/pm/model";
import { Node } from "@tiptap/core";

export default function isNode(value: any): value is Node {
  return (
    value instanceof ProseNode && (value as any as Node).type !== undefined
  );
}
