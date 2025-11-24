// TaskItemNodeView.tsx
import {
  NodeViewWrapper,
  NodeViewContent,
  ReactNodeViewProps,
  ReactNodeViewRenderer,
} from "@tiptap/react";
import { Checkbox } from "@/components/ui/checkbox";
import {
  TableCellOptions,
  TableHeaderOptions,
  TableOptions,
  TableRowOptions,
  TableCell as TableCellNode,
  TableHeader as TableHeaderNode,
  TableRow as TableRowNode,
  Table as TableNode,
} from "@tiptap/extension-table";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import React, { useMemo, useRef } from "react";
import { Node } from "@tiptap/pm/model";
import { createPortal } from "react-dom";

export function TableNodeView({
  node,
}: ReactNodeViewProps & {
  options: TableOptions;
}) {
  const { header, rows } = useMemo(() => {
    const header = node.children.find(
      (child) => child.type.name == TableHeaderNode.name
    );

    const rows = node.children.filter(
      (child) => child.type.name == TableRowNode.name
    );

    return { header, rows };
  }, [node]);

  return (
    <NodeViewWrapper as={TableCell}>
      <Table>
        <TableBody>
          {header && <TableHeaderNodeView node={header} />}{" "}
          {rows.map((row, index) => (
            <TableRowNodeView
              key={index}
              node={row}
              rendererProps={rendererProps}
            />
          ))}
        </TableBody>
      </Table>
    </NodeViewWrapper>
  );
}

export function TableRowNodeView({
  node,
  rendererProps,
}: {
  node: Node;
  rendererProps: ReactNodeViewProps;
}) {
  const cells = Array.from(node.children)
    .filter(
      (child) =>
        child.type.name == TableCellNode.name ||
        child.type.name == TableHeaderNode.name
    )
    .map(() => {
      const nodeView = ReactNodeViewRenderer(TableCellNodeView)({
        ...rendererProps,
        node: node,
      });
      return createPortal(
        <TableCellNodeView node={node} />,
        nodeView.dom as HTMLElement
      );
    });

  console.log(node);
  return (
    <TableRow>
      {cells.map((cell, index) => (
        <React.Fragment key={index}>{cell}</React.Fragment>
      ))}
    </TableRow>
  );
}

export function TableHeaderNodeView({ node }: { node: Node }) {
  return (
    <TableHead
      contentEditable
      onChange={(e) => {
        console.log(e);
      }}
    >
      {node.text}
    </TableHead>
  );
}

export function TableCellNodeView({ node }: ReactNodeViewProps) {
  return (
    <TableCell
      contentEditable
      onChange={(e) => {
        console.log(e);
      }}
    ></TableCell>
  );
}
