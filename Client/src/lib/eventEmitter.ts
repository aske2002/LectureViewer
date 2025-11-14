
export class PrivateEventEmitter<
  T extends {
    [key: string]: any[] | never;
  }
> {
  protected handlers: {
    [K in keyof T]?: ((...value: T[K]) => void)[];
  } = {};

  constructor() {
    this.handlers = {};
  }

  // Overloads to handle "never" types
  protected emit<K extends keyof T>(event: K, value: T[K]): void;
  protected emit<K extends keyof T>(event: K): void;
  protected emit<K extends keyof T>(event: K, value?: T[K]): void {
    const handlers = this.handlers[event] as ((...value: T[K]) => void)[] | [];
    for (const h of handlers) {
      h(...(value! || []));
    }
  }

  public on<K extends keyof T>(
    event: K,
    handler: (...value: T[K]) => void
  ): void {
    if (!this.handlers[event]) {
      this.handlers[event] = [handler];
    } else {
      this.handlers[event].push(handler);
    }
  }

  public once<K extends keyof T>(
    event: K,
    handler: (...value: T[K]) => void
  ): void {
    const onceHandler = (...value: T[K]) => {
      handler(...value);
      this.off(event, onceHandler);
    };
    this.on(event, onceHandler);
  }

  public off<K extends keyof T>(
    event: K,
    handler: (...value: T[K]) => void
  ): void {
    this.handlers[event] = this.handlers[event]?.filter((h) => h !== handler);
  }

  public dispose(): void {
    this.handlers = {};
  }
}

export class EventEmitter<
  T extends {
    [key: string]: any[] | never;
  }
> extends PrivateEventEmitter<T> {
  constructor() {
    super();
  }
  // Overloads to handle "never" types
  public emit<K extends keyof T>(event: K, value: T[K]): void;
  public emit<K extends keyof T>(event: K): void;
  public emit<K extends keyof T>(event: K, value?: T[K]): void {
    super.emit<K>(event, value as T[K]);
  }
}
