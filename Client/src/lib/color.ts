// color.ts — A modern color utility class with OKLCH support

export type RGB = { r: number; g: number; b: number; a?: number };
export type OKLCH = { l: number; c: number; h: number; a?: number };

export class Color {
  private _rgb: RGB;

  constructor(rgb: RGB) {
    this._rgb = { ...rgb, a: rgb.a ?? 1 };
  }

  // -----------------------------
  // Static Constructors
  // -----------------------------

  static fromHex(hex: string): Color {
    const { r, g, b, a } = Color.parseHex(hex);
    return new Color({ r, g, b, a: a / 255 });
  }

  static fromRGB(r: number, g: number, b: number, a: number = 1): Color {
    return new Color({ r, g, b, a });
  }

  static fromOKLCHString(oklchString: string): Color {
    const match = oklchString
      .trim()
      .match(
        /^oklch\(\s*([\d.]+)\s+([\d.]+)\s+([\d.]+)(?:\s*\/\s*([\d.]+))?\s*\)$/i
      );
    if (!match) {
      console.error(`Invalid OKLCH color string: ${oklchString}`);
      return Color.fromOKLCH(0, 0, 0);
    }
    const l = parseFloat(match[1]);
    const c = parseFloat(match[2]);
    const h = parseFloat(match[3]);
    const a = match[4] !== undefined ? parseFloat(match[4]) : 1;
    return Color.fromOKLCH(l, c, h, a);
  }

  static fromOKLCH(l: number, c: number, h: number, a: number = 1): Color {
    const rgb = Color.oklchToRgb({ l, c, h, a });
    return new Color(rgb);
  }

  withAlpha(alpha: number): Color {
    return Color.fromRGB(this._rgb.r, this._rgb.g, this._rgb.b, alpha);
  }

  lighten(amount: number): Color {
    const oklch = this.toOKLCH();
    const l = Math.min(1, Math.max(0, oklch.l + amount));
    return Color.fromOKLCH(l, oklch.c, oklch.h, oklch.a);
  }

  /**
   * Darkens a color (syntactic sugar)
   */
  darken(amount: number): Color {
    return this.lighten(-amount);
  }

  // -----------------------------
  // Conversion Methods
  // -----------------------------

  toHex(): string {
    const { r, g, b, a = 1 } = this._rgb;
    const rr = Math.round(r).toString(16).padStart(2, "0");
    const gg = Math.round(g).toString(16).padStart(2, "0");
    const bb = Math.round(b).toString(16).padStart(2, "0");
    const aa =
      a < 1
        ? Math.round(a * 255)
            .toString(16)
            .padStart(2, "0")
        : "";
    return `#${rr}${gg}${bb}${aa}`;
  }

  toRGB(): RGB {
    return { ...this._rgb };
  }

  toOKLCH(): OKLCH {
    return Color.rgbToOklch(this._rgb);
  }

  // -----------------------------
  // Internal conversion logic
  // -----------------------------

  private static parseHex(input: string) {
    let hex = input.trim().replace(/^#/, "");
    if (hex.length === 3 || hex.length === 4) {
      hex = hex
        .split("")
        .map((c) => c + c)
        .join("");
    }
    if (hex.length !== 6 && hex.length !== 8) {
      console.error(`Invalid hex color: ${input}`);
      return { r: 0, g: 0, b: 0, a: 1 };
    }
    const r = parseInt(hex.slice(0, 2), 16);
    const g = parseInt(hex.slice(2, 4), 16);
    const b = parseInt(hex.slice(4, 6), 16);
    const a = hex.length === 8 ? parseInt(hex.slice(6, 8), 16) : 255;
    return { r, g, b, a };
  }

  private static srgbToLinear(v: number): number {
    return v <= 0.04045 ? v / 12.92 : Math.pow((v + 0.055) / 1.055, 2.4);
  }

  private static linearToSrgb(v: number): number {
    return v <= 0.0031308 ? v * 12.92 : 1.055 * Math.pow(v, 1 / 2.4) - 0.055;
  }

  // RGB (0–255) → OKLCH
  private static rgbToOklch(rgb: RGB): OKLCH {
    const r = Color.srgbToLinear(rgb.r / 255);
    const g = Color.srgbToLinear(rgb.g / 255);
    const b = Color.srgbToLinear(rgb.b / 255);

    const l = 0.4122214708 * r + 0.5363325363 * g + 0.0514459929 * b;
    const m = 0.2119034982 * r + 0.6806995451 * g + 0.1073969566 * b;
    const s = 0.0883024619 * r + 0.2817188376 * g + 0.6299787005 * b;

    const l_ = Math.cbrt(l);
    const m_ = Math.cbrt(m);
    const s_ = Math.cbrt(s);

    const L = 0.2104542553 * l_ + 0.793617785 * m_ - 0.0040720468 * s_;
    const A = 1.9779984951 * l_ - 2.428592205 * m_ + 0.4505937099 * s_;
    const B = 0.0259040371 * l_ + 0.7827717662 * m_ - 0.808675766 * s_;

    const C = Math.hypot(A, B);
    let H = Math.atan2(B, A) * (180 / Math.PI);
    if (H < 0) H += 360;

    return { l: L, c: C, h: H, a: rgb.a };
  }

  // OKLCH → RGB (0–255)
  private static oklchToRgb({ l, c, h, a = 1 }: OKLCH): RGB {
    const hr = (h * Math.PI) / 180;
    const A = Math.cos(hr) * c;
    const B = Math.sin(hr) * c;

    const l_ = Math.pow(l + 0.3963377774 * A + 0.2158037573 * B, 3);
    const m_ = Math.pow(l - 0.1055613458 * A - 0.0638541728 * B, 3);
    const s_ = Math.pow(l - 0.0894841775 * A - 1.291485548 * B, 3);

    const r = +4.0767416621 * l_ - 3.3077115913 * m_ + 0.2309699292 * s_;
    const g = -1.2684380046 * l_ + 2.6097574011 * m_ - 0.3413193965 * s_;
    const b = -0.0041960863 * l_ - 0.7034186147 * m_ + 1.707614701 * s_;

    return {
      r: Math.min(255, Math.max(0, Color.linearToSrgb(r) * 255)),
      g: Math.min(255, Math.max(0, Color.linearToSrgb(g) * 255)),
      b: Math.min(255, Math.max(0, Color.linearToSrgb(b) * 255)),
      a,
    };
  }
}
