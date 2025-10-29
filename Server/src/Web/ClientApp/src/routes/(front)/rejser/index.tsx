import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/(front)/rejser/')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/rejser/"!</div>
}
