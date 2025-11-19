export function parseIntOrDefault(
  value: string,
  defaultValue: number = 0
): number {
  const parsed = parseInt(value, 10);
  return isNaN(parsed) ? defaultValue : parsed;
}

export function parseFloatOrDefault(
  value: string,
  defaultValue: number = 0
): number {
  const parsed = parseFloat(value);
  return isNaN(parsed) ? defaultValue : parsed;
}

export function parseTimecodeToSeconds(timecode: string): number {
  const parts = timecode.split(":").map((part) => parseFloat(part));
    if (parts.length !== 3) {
    return 0;
  }
  const [hours, minutes, seconds] = parts;
  return hours * 3600 + minutes * 60 + seconds;
}