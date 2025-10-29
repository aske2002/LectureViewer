import { buttonVariants } from "../ui/button";
import { HeartIcon } from "lucide-react";
import { VariantProps } from "class-variance-authority";
import LinkButton from "./link-button";

export default function DonateButton({
  children,
  variant,
  size,
}: {
  size?: VariantProps<typeof buttonVariants>["size"];
  variant?: VariantProps<typeof buttonVariants>["variant"];
  children: React.ReactNode;
}) {
  return (
    <LinkButton
      className="group"
      variant={variant}
      size={size}
      to="/doner"
      hash="now"
      hashScrollIntoView={true}
    >
      <HeartIcon className="mr-2 h-4 w-4 fill-transparent group-hover:fill-current transition-all duration-400 group-hover:animate-beat" />
      {children}
    </LinkButton>
  );
}
