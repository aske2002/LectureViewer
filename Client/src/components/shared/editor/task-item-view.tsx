// TaskItemNodeView.tsx
import { NodeViewWrapper, NodeViewContent, ReactNodeViewProps } from '@tiptap/react'
import { Checkbox } from "@/components/ui/checkbox"

export default function TaskItemNodeView({ node, updateAttributes }: ReactNodeViewProps) {
  const checked = node.attrs.checked

  return (
    <NodeViewWrapper class="flex gap-2 items-center">
      <Checkbox
        checked={checked}
        onCheckedChange={() => updateAttributes({ checked: !checked })}
      />

      <div className="flex-1">
        <NodeViewContent />
      </div>
    </NodeViewWrapper>
  )
}
