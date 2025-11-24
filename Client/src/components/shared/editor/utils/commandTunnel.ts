import { createContext } from "react";
import type tunnel from "tunnel-rat";

export const EditorCommandTunnelContext = createContext(
  {} as ReturnType<typeof tunnel>
);