import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/(front)/kontakt/')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/kontakt/"!</div>
}
