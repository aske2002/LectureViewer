import { spawn, SpawnOptions } from "child_process";

export interface SpawnAsyncOptions {
  spawnOptions?: SpawnOptions;
  onStdout?: (data: string) => void;
  onStderr?: (data: string) => void;
  pipeStdout?: boolean;
  pipeStderr?: boolean;
}

export interface SpawnAsyncResult {
  stdout: string;
  stderr: string;
  code: number;
}

/**
 * Asynchronously spawns a child process with optional output handlers and logging.
 */
export async function spawnAsync(
  command: string,
  args: string[] = [],
  {
    spawnOptions = {},
    onStdout,
    onStderr,
    pipeStdout = false,
    pipeStderr = false,
  }: SpawnAsyncOptions = {}
): Promise<SpawnAsyncResult> {
  return new Promise((resolve, reject) => {
    const child = spawn(command, args, {
      shell: true,
      ...spawnOptions,
    });

    let stdout = "";
    let stderr = "";

    if (child.stdout) {
      child.stdout.setEncoding('utf8');
      child.stdout.on("data", (data: Buffer) => {
        const text = data.toString('utf8');
        stdout += text;
        if (onStdout) onStdout(text);
        if (pipeStdout) process.stdout.write(text);
      });
    }

    if (child.stderr) {
      child.stderr.setEncoding('utf8');
      child.stderr.on("data", (data: Buffer) => {
        const text = data.toString('utf8');
        stderr += text;
        if (onStderr) onStderr(text);
        if (pipeStderr) process.stderr.write(text);
      });
    }

    child.on("error", (err) => {
      reject(err);
    });

    child.on("close", (code) => {
      resolve({
        stdout,
        stderr,
        code: code ?? 0,
      });
    });
  });
}
