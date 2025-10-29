import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";

declare global {
  interface String {
    capitalizeFirstLetter(): string;
  }
}

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

String.prototype.capitalizeFirstLetter = function () {
  return this.charAt(0).toUpperCase() + this.slice(1);
};
