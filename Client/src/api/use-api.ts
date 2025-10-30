import { ApiProviderContext } from "@/context/api-provider-context";
import { useContext } from "react";

export default function useApi() {
    return useContext(ApiProviderContext);
}