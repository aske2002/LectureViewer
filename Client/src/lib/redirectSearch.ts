import { fallback } from "@tanstack/zod-adapter";
import { z } from "zod";

export const redirectSearchsearch = z.object({
  redirect: fallback(z.string(), "/").default("/"),
});
