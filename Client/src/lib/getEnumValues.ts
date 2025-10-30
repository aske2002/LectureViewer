export function getEnumKeys<T extends Record<string, string | number>>(
  enumObj: T
): (keyof T)[] {
  return Object.keys(enumObj).filter((key) => isNaN(Number(key))) as (keyof T)[];
}

export function getEnumValues<T extends Record<string, string | number>>(
  enumObj: T
): T[keyof T][] { 
    return Object.values(enumObj).filter((value) => isNaN(Number(value))) as T[keyof T][];
}

export function getEnumEntries<T extends Record<string, string | number>>(
  enumObj: T
): [keyof T, T[keyof T]][] {
  return getEnumKeys(enumObj).map((key) => [key, enumObj[key]]);
}