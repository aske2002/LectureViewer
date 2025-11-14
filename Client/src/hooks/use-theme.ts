import { Color } from "@/lib/color";
import { useMemo } from "react";

export default function useTheme() {
  return useMemo(() => {
    const styles = getComputedStyle(document.documentElement);
    return {
      primary: Color.fromOKLCHString(styles.getPropertyValue("--primary")),
      background: Color.fromOKLCHString(styles.getPropertyValue("--background")),
      card: Color.fromOKLCHString(styles.getPropertyValue("--card")),
      border: Color.fromOKLCHString(styles.getPropertyValue("--border")),
      primaryForeground: Color.fromOKLCHString(styles.getPropertyValue("--primary-foreground")),
    };
  }, []);
}
