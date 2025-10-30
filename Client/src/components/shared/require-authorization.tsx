import usePolicyRoles from "@/hooks/use-authorization";

type RequirePolicyRoleProps = Parameters<typeof usePolicyRoles>[0] & {
  children: React.ReactNode;
  ref?: React.Ref<HTMLDivElement>;
};

export default function RequireAuthorization({
  children,
  ...props
}: RequirePolicyRoleProps) {
  const isAuthorized = usePolicyRoles(props);
  return isAuthorized ? children : null;
}
