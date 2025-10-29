import { RadioGroupItem } from "@radix-ui/react-radio-group";
import { Label } from "../ui/label";

interface DonateValueProps {
  value: number;
}

export default function DonateValue({ value }: DonateValueProps) {
  return (
    <div>
      <RadioGroupItem
        value={value.toString()}
        id={`r${value}`}
        className="peer sr-only"
      />
      <Label
        htmlFor={`r${value}`}
        className="flex flex-col cursor-pointer items-center justify-between rounded-md border-2 border-muted bg-popover p-4 hover:bg-accent hover:text-accent-foreground peer-data-[state=checked]:border-primary [&:has([data-state=checked])]:border-primary"
      >
        <span className="text-xl font-bold">{value} kr</span>
      </Label>
    </div>
  );
}
