import useAuth from "./use-auth-client";
import { useMemo } from "react";
import { PolicyDto, RoleDto } from "@/api/web-api-client";

export default function usePolicyRoles({
  allowRoles,
  allowPolicies
}: {
  allowRoles?: RoleDto[];
  allowPolicies?: PolicyDto[];
}) {
  const userData = useAuth();
  return useMemo(() => {
    if (!userData.user) return false;

    const userRoles = userData.user.roles || [];
    const userPolicies = userData.user.policies || [];

    const hasRequiredRoles =
      allowRoles?.some((role) => userRoles.includes(role)) ?? false;
    const hasRequiredPolicies =
      allowPolicies?.some((policy) => userPolicies.includes(policy)) ?? false;

    return hasRequiredRoles || hasRequiredPolicies;
  }, [userData, allowRoles, allowPolicies]);
}
