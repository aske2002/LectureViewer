import {
  Editor,
  Extension,
  findParentNode,
  findParentNodeClosestToPos,
} from "@tiptap/core";

import { Node } from "@tiptap/pm/model";
import { Plugin, PluginKey } from "prosemirror-state";
import { Decoration, DecorationSet } from "prosemirror-view";
import { TableMap } from "prosemirror-tables";

type ColItem = {
  node: Node;
  pos: number;
  index: number;
};

type RowItem = {
  node: Node;
  pos: number;
  index: number;
};

const getCols = (
  table: Node,
  tablePos: number,
  tableMap: TableMap,
  row = 0
): ColItem[] => {
  const cols: ColItem[] = [];

  for (let colIndex = 0; colIndex < tableMap.width; colIndex++) {
    const colPos = tableMap.positionAt(row, colIndex, table);
    const colNode = table.nodeAt(colPos);
    if (colNode) {
      cols.push({
        node: colNode,
        pos: tablePos + 1 + colPos,
        index: colIndex,
      });
    }
  }
  return cols;
};

const getRows = (
  table: Node,
  tablePos: number,
  tableMap: TableMap,
  col = 0
): RowItem[] => {
  const rows: RowItem[] = [];

  for (let rowIndex = 0; rowIndex < tableMap.height; rowIndex++) {
    const rowPos = tableMap.positionAt(rowIndex, col, table);
    const rowNode = table.nodeAt(rowPos);
    if (rowNode) {
      rows.push({
        node: rowNode,
        pos: tablePos + 1 + rowPos,
        index: rowIndex,
      });
    }
  }
  return rows;
};

const createRowDecoration = (rowItem: RowItem, editor: Editor) => {
  const parentRow = findParentNodeClosestToPos(
    editor.state.doc.resolve(rowItem.pos),
    (node) => node.type.name === "tableRow"
  );

  if (!parentRow) return;

  const dragRow = document.createElement("div");
  dragRow.className = "tiptap-table-add tiptap-row-drag";
  dragRow.title = "Add row";
  const dragRowButton = document.createElement("button");
  dragRow.appendChild(dragRowButton);
  dragRowButton.className = "tiptap-row-add-button";
  dragRowButton.textContent = "⋯";

  dragRow.addEventListener("click", (e) => {
    e.preventDefault();
    editor.chain().focus(rowItem.pos).addRowAfter().run();
  });

  return Decoration.widget(
    parentRow.pos + parentRow.node.nodeSize,
    () => dragRow,
    {
      side: 1,
      stopEvent: () => true,
    }
  );
};

const createColumnDeclaration = (
  table: Node,
  tableMap: TableMap,
  tablePos: number,
  columnItem: ColItem,
  editor: Editor,
  cols: ColItem[]
) => {
  const colRows = getRows(table, tablePos, tableMap, columnItem.index);
  const leftOffset = cols.reduce((acc, col) => {
    if (col.index <= columnItem.index) {
      return acc + (col.node.attrs.colwidth?.[0] || 100);
    }
    return acc;
  }, 0);

  const dragColWrapper = document.createElement("div");
  dragColWrapper.className = "tiptap-table-add tiptap-col-drag-wrapper";
  dragColWrapper.style.left = `${leftOffset}px`;

  const dragCol = document.createElement("div");
  dragColWrapper.appendChild(dragCol);
  dragCol.className = "tiptap-col-drag";
  dragCol.title = "Add column or drag to resize";

  const dragColButton = document.createElement("button");
  dragColWrapper.appendChild(dragColButton);
  dragColButton.className = "tiptap-col-add-button";
  dragColButton.textContent = "⋮";

  let dragInfo: {
    xStart: number;
    xMove: number;
    originalWidth: number;
  } | null = null;

  const handleMouseUp = (e: MouseEvent) => {
    e.preventDefault();
    if (dragInfo && dragInfo.xMove == 0) {
      editor.chain().focus(columnItem.pos).addColumnAfter().run();
    }
    dragInfo = null;
  };

  const handleMouseDown = (e: MouseEvent) => {
    e.preventDefault();
    dragInfo = {
      xStart: e.clientX,
      xMove: 0,
      originalWidth: columnItem.node.attrs.colwidth[0] || 100,
    };
  };

  const handleMouseMove = (e: MouseEvent) => {
    e.preventDefault();
    if (dragInfo !== null) {
      const deltaX = e.clientX - dragInfo.xStart;
      console.log("DeltaX:", deltaX);
      editor
        .chain()
        .focus()
        .command(({ tr }) => {
          for (const colRow of colRows) {
            const resolvedCol = tr.doc.nodeAt(colRow.pos);
            if (!resolvedCol) continue;

            tr.setNodeMarkup(colRow.pos, resolvedCol.type, {
              colwidth: [(dragInfo?.originalWidth || 0) + deltaX],
            });
          }
          return true;
        })
        .run();
      dragInfo.xMove = deltaX;
    }
  };

  window.addEventListener("mouseup", (e) => {
    handleMouseUp(e);
  });
  window.addEventListener("mousemove", (e) => {
    handleMouseMove(e);
  });
  dragColWrapper.onmousedown = (e) => {
    handleMouseDown(e);
  };

  return Decoration.widget(tablePos + table.nodeSize - 1, dragColWrapper, {
    side: 1,
    stopEvent: () => true,
    destroy: () => {
      window.removeEventListener("mouseup", handleMouseUp);
      window.removeEventListener("mousemove", handleMouseMove);
      dragCol.onmousedown = null;
    },
  });
};

export const TableAddButtons = Extension.create({
  name: "tableAddButtons",
  addKeyboardShortcuts() {
    return {
      Backspace: () => {
        const editor = this.editor;
        const { state } = editor;
        const { selection } = state;
        const { $from } = selection;

        // Not inside a cell → normal backspace
        const cellNode = $from.node(-1);
        if (
          !cellNode ||
          !["tableCell", "tableHeader"].includes(cellNode.type.name)
        ) {
          return false;
        }

        // Not at start of cell → normal backspace
        if ($from.parentOffset > 0) return false;

        // Find the table
        const $cellPos = state.doc.resolve($from.before());
        const table = findParentNode((node) => node.type.name === "table")(
          selection
        );
        if (!table) return false;

        const map = TableMap.get(table.node);
        const col = map.colCount($cellPos.pos - table.start - 1);

        if (col === 0 && editor.can().deleteRow()) {
          return editor.commands.deleteRow();
        }

        if (editor.can().deleteColumn()) {
          return editor.commands.deleteColumn();
        }

        return false;
      },
    };
  },
  addProseMirrorPlugins() {
    const editor = this.editor;

    return [
      new Plugin({
        key: new PluginKey("tableAddButtons"),

        props: {
          decorations(state) {
            const { doc } = state;
            const decorations: any[] = [];

            doc.descendants((node, pos) => {
              if (node.type.name === "table") {
                const tableMap = TableMap.get(node);

                if (tableMap.width === 0 || tableMap.height === 0) {
                  return;
                }

                const cols = getCols(node, pos, tableMap);
                const rows = getRows(node, pos, tableMap);

                rows.forEach((item) => {
                  decorations.push(createRowDecoration(item, editor));
                });

                cols.forEach((item) => {
                  decorations.push(
                    createColumnDeclaration(
                      node,
                      tableMap,
                      pos,
                      item,
                      editor,
                      cols
                    )
                  );
                });
              }
            });

            return DecorationSet.create(doc, decorations);
          },
        },
      }),
    ];
  },
});
