import z from "zod";

export const zodNumberFromString = z.preprocess((val) => {
  if (typeof val === "string") {
    const parsed = Number(val);
    if (!isNaN(parsed)) {
      return parsed;
    }
  }
  return val;
}, z.number());
