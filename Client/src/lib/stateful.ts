
export interface State<T> {
  onDispose?(): void;
  onSubscribe?(listener: (value: T) => void): void;
}

export type ValueTypes =
  | object
  | string
  | number
  | boolean
  | symbol
  | bigint
  | null
  | undefined;

export class State<T> {
  private value: T;
  private listeners: Array<(value: T) => void> = [];

  public constructor(initialValue: T) {
    this.value = initialValue;
  }

  public getValue = () => {
    return this.value;
  }

  public setValue = (newValue: T) => {
    this.value = newValue;
    for (const listener of this.listeners) {
      listener(newValue);
    }
  }

  public onChange(listener: (value: T) => void) {
    this.listeners.push(listener);
    this.onSubscribe && this.onSubscribe(listener);
    return () => {
      const index = this.listeners.indexOf(listener);
      if (index > -1) {
        this.listeners.splice(index, 1);
      }
    };
  }

  public offChange(listener: (value: T) => void) {
    const index = this.listeners.indexOf(listener);
    if (index > -1) {
      this.listeners.splice(index, 1);
    }
  }

  public dispose() {
    this.onDispose && this.onDispose();
    this.listeners.splice(0);
  }
}
