import { VariantProps } from "class-variance-authority";
import { buttonVariants } from "../ui/button";
import { cn } from "@/lib/utils";
import {
  AnyRouter,
  Link,
  LinkComponentProps,
  RegisteredRouter,
} from "@tanstack/react-router";

export default function LinkButton<
  TRouter extends AnyRouter = RegisteredRouter,
  const TFrom extends string = string,
  const TTo extends string | undefined = undefined,
  const TMaskFrom extends string = TFrom,
  const TMaskTo extends string = ""
>({
  variant,
  size,
  className,
  ...props
}: LinkComponentProps<"a", TRouter, TFrom, TTo, TMaskFrom, TMaskTo> &
  VariantProps<typeof buttonVariants>) {
  return (
    <Link
      className={cn(buttonVariants({ variant, size, className }))}
      {...(props as LinkComponentProps<
        "a",
        TRouter,
        TFrom,
        TTo,
        TMaskFrom,
        TMaskTo
      >)}
    />
  );
}
