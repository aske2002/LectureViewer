import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/(front)/rejser/$tripId')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/rejser/[id]"!</div>
}
