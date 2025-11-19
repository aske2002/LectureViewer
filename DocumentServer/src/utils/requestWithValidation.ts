import { Request, Response, RequestHandler } from "express-serve-static-core";
import z from "zod";

export const requestWithValidation = <T>(
  schema: z.ZodSchema<T>,
  handler: (body: T, ...rest: Parameters<RequestHandler>) => any 
): RequestHandler => {
  return async (...args) => {
    try {
      const parsedBody = schema.parse(args[0].body);
      await handler(parsedBody, ...args);
    } catch (err: any) {
      args[1].status(400).json({ error: err.errors || err.message });
    }
  };
};
