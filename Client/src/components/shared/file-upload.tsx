// components/FileUpload.tsx
import { useCallback, useEffect, useMemo, useState } from "react";
import { useFileUpload } from "@/api/use-file-upload";
import { FileAudio, FileText, FileVideo, Upload, X } from "lucide-react";
import { cn } from "@/lib/utils";
import { EventEmitter } from "@/lib/eventEmitter";
import { State } from "@/lib/stateful";
import { Button } from "../ui/button";

type UseFileUploadArgs<TClient, TReturn, TMetadata extends Record<string,any>> = Parameters<
  typeof useFileUpload<
    TClient,
    TReturn,
    {
      file: File;
      metadata?: TMetadata;
    }
  >
>[0] & {
  initialMetadata?: TMetadata | ((file: File) => TMetadata);
};

export type FileFieldProps<TClient, TReturn, TMetadata extends Record<string, any>> = UseFileUploadArgs<
  TClient,
  TReturn,
  TMetadata
>;

class FileFieldController<TReturn, TMetadata extends Record<string, any>> {
  public readonly progress: State<number> = new State(0);
  public readonly isPending: State<boolean> = new State(false);
  public readonly files: State<FileUploadController<TReturn, TMetadata>[]> =
    new State<FileUploadController<TReturn, TMetadata>[]>([]);

  constructor(
    private options?: Partial<FileFieldProps<any, TReturn, TMetadata>>
  ) {}

  public dispose = () => {
    for (const fileController of this.files.getValue()) {
      fileController.dispose();
    }
    this.progress.dispose();
    this.isPending.dispose();
    this.files.dispose();
  }

  public reset = () => {
    for (const fileController of this.files.getValue()) {
      fileController.dispose();
    }
    this.files.setValue([]);
    this.progress.setValue(0);
    this.isPending.setValue(false);
  }

  private onFileProgressChanged = () => {
    const totalFiles = this.files.getValue().length;
    if (totalFiles === 0) {
      this.progress.setValue(0);
      return;
    }

    const totalProgress = this.files
      .getValue()
      .reduce(
        (acc, fileController) => acc + fileController.progress.getValue(),
        0
      );

    this.progress.setValue(Math.round(totalProgress / totalFiles));
  };

  private onFilePendingChanged = () => {
    const anyPending = this.files
      .getValue()
      .some((fileController) => fileController.isPending.getValue());
    this.isPending.setValue(anyPending);
  };

  private onFileRemoved = (
    fileController: FileUploadController<TReturn, TMetadata>
  ) => {
    fileController.dispose();
    const currentFiles = this.files
      .getValue()
      .filter((fc) => fc !== fileController);
    this.files.setValue(currentFiles);
    this.onFileProgressChanged();
    this.onFilePendingChanged();
  };

  public upload = (): Promise<TReturn[]> => {
    const uploadPromises = this.files
      .getValue()
      .map((fileController) => fileController.upload());

    return Promise.all(uploadPromises)
      .then((results) => {
        return results;
      })
      .finally(() => {
        this.isPending.setValue(false);
      });
  };

  public get totalSize(): number {
    return this.files
      .getValue()
      .reduce(
        (acc, fileController) => acc + fileController.file.getValue().size,
        0
      );
  }

  public add(file: File) {
    const currentFiles = this.files.getValue();
    const newFileController = new FileUploadController<TReturn, TMetadata>(
      file,
      typeof this.options?.initialMetadata === "function"
        ? this.options.initialMetadata(file)
        : this.options?.initialMetadata
    );

    newFileController.progress.onChange(this.onFileProgressChanged);
    newFileController.isPending.onChange(this.onFilePendingChanged);
    newFileController.on("remove", () => this.onFileRemoved(newFileController));

    this.files.setValue([...currentFiles, newFileController]);
    return newFileController;
  }

  public addRange(files: File[]) {
    for (const file of files) {
      this.add(file);
    }
  }
}

class FileUploadController<
  TReturn,
  TMetadata extends any,
> extends EventEmitter<{
  upload: never;
  uploaded: [result: TReturn];
  uploadFailed: [error: Error];
  remove: never;
}> {
  public readonly file: State<File>;
  public readonly progress: State<number> = new State(0);
  public readonly isPending: State<boolean> = new State(false);
  public readonly metadata: State<TMetadata | undefined> = new State<
    TMetadata | undefined
  >(undefined);

  constructor(file: File, metadata?: TMetadata) {
    super();
    this.file = new State(file);
    this.metadata = new State(metadata);
  }

  public upload = async (): Promise<TReturn> => {
    return new Promise<TReturn>((resolve, reject) => {
      this.emit("upload");

      const handleUploadFailed = (error: Error) => {
        this.off("uploaded", handleUploaded);
        this.off("uploadFailed", handleUploadFailed);
        reject(error);
      };

      const handleUploaded = (result: TReturn) => {
        this.off("uploaded", handleUploaded);
        this.off("uploadFailed", handleUploadFailed);
        resolve(result);
      };

      this.on("uploaded", handleUploaded);
      this.on("uploadFailed", handleUploadFailed);
    });
  };

  public dispose() {
    this.file.dispose();
    this.progress.dispose();
    this.isPending.dispose();
    this.metadata.dispose();
  }

  public remove() {
    this.emit("remove");
  }
}

type RenderFileMethod<TReturn, TMetadata> = React.ComponentType<{
  file: File;
  controller: FileUploadController<TReturn, TMetadata>;
  metadata: TMetadata | undefined;
  setMetadata: (metadata: TMetadata | undefined) => void;
}>;

export function useFileField<
  TClient,
  TReturn,
  TMetadata extends Record<string, any>,
>(options: FileFieldProps<TClient, TReturn, TMetadata>) {
  const [controller] = useState(() => {
    return new FileFieldController<TReturn, TMetadata>(options);
  });

  useEffect(() => {
    return () => {
      controller.dispose();
    };
  }, [controller]);

  const controllerToFile = (
    controller: FileUploadController<TReturn, TMetadata>
  ) => controller.file.getValue();

  const [progress, setProgress] = useState(controller.progress.getValue());
  const [isPending, setIsPending] = useState(controller.isPending.getValue());
  const [files, setFiles] = useState<File[]>(
    controller.files.getValue().map(controllerToFile)
  );

  useEffect(() => {
    const handleFilesChange = (
      files: FileUploadController<TReturn, TMetadata>[]
    ) => {
      setFiles(files.map(controllerToFile));
    };

    controller.progress.onChange(setProgress);
    controller.isPending.onChange(setIsPending);
    controller.files.onChange(handleFilesChange);

    return () => {
      controller.progress.offChange(setProgress);
      controller.isPending.offChange(setIsPending);
      controller.files.offChange(handleFilesChange);
    };
  }, [controller]);

  return {
    ...options,
    controller,
    upload: controller.upload.bind(controller),
    reset: controller.reset.bind(controller),
    files,
    progress,
    isPending,
  };
}

export function FileUploadField<TClient, TReturn, TMetadata extends Record<string, any>>({
  controller,
  className,
  color,
  ...props
}: FileFieldProps<TClient, TReturn, TMetadata> & {
  className?: string;
  controller: FileFieldController<TReturn, TMetadata>;
  RenderFile?: RenderFileMethod<TReturn, TMetadata>;
  color?: string;
}) {
  const [isHoveringInput, setIsHoveringInput] = useState(false);
  const [files, setFiles] = useState<
    FileUploadController<TReturn, TMetadata>[]
  >(controller.files.getValue());

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      controller.addRange(Array.from(e.target.files));
    }
  };

  useEffect(() => {
    controller.files.onChange(setFiles);

    return () => {
      controller.files.offChange(setFiles);
    };
  }, []);
  
  return (
    <div className={cn("space-y-2", className)}>
      <div
        className="border-2 border-dashed border-border rounded-lg p-8 text-center transition-colors"
        onMouseEnter={() => setIsHoveringInput(true)}
        onMouseLeave={() => setIsHoveringInput(false)}
        style={{
          borderColor: isHoveringInput ? color : undefined,
        }}
      >
        <input
          id="files"
          type="file"
          multiple
          onChange={handleFileChange}
          className="hidden"
        />
        <label htmlFor="files" className="cursor-pointer">
          <Upload
            className="h-10 w-10 mx-auto mb-3 text-muted-foreground"
            style={{
              color: isHoveringInput ? color : undefined,
            }}
          />
          <p className="text-sm font-medium mb-1">
            Click to upload or drag and drop
          </p>
          <p className="text-xs text-muted-foreground">
            Audio, video, slides (PDF, PPT)
          </p>
        </label>
      </div>
      {files.length > 0 && (
        <div className="mt-4 space-y-2">
          {files.map((file, index) => (
            <FileUpload
              key={index}
              controller={file}
              color={color}
              {...props}
            />
          ))}
        </div>
      )}
    </div>
  );
}

export function FileUpload<TClient, TReturn, TMetadata extends Record<string, any>>({
  controller,
  color,
  RenderFile,
  ...props
}: {
  controller: FileUploadController<TReturn, TMetadata>;
  RenderFile?: RenderFileMethod<TReturn, TMetadata>;
  color?: string;
} & UseFileUploadArgs<TClient, TReturn, TMetadata>) {
  const [file, setFile] = useState<File>(controller.file.getValue());
  const [metadata, _setMetadata] = useState<TMetadata | undefined>(
    controller.metadata.getValue()
  );
  const { mutateAsync: upload, isPending, progress } = useFileUpload(props);

  useEffect(() => {
    controller.progress.setValue(progress);
  }, [progress]);

  useEffect(() => {
    controller.isPending.setValue(isPending);
  }, [isPending]);

  useEffect(() => {
    const doUpload = async () => {
      try {
        const result = await upload({ file, metadata });
        controller.emit("uploaded", [result]);
      } catch (e) {
        if (!(e instanceof Error)) {
          controller.emit("uploadFailed", [new Error(String(e))]);
        } else {
          controller.emit("uploadFailed", [e]);
        }
      }
    };

    controller.on("upload", doUpload);

    return () => {
      controller.off("upload", doUpload);
    };
  }, [metadata, upload]);

  useEffect(() => {
    controller.file.onChange(setFile);
    controller.metadata.onChange(_setMetadata);

    return () => {
      controller.file.offChange(setFile);
      controller.metadata.offChange(_setMetadata);
    };
  }, [controller]);

  const setMetadata = useCallback(
    (metadata: TMetadata | undefined) => {
      controller.metadata.setValue(metadata);
    },
    [controller]
  );

  if (RenderFile) {
    return <RenderFile {...{ file, controller, metadata, setMetadata }} />;
  }

  return (
    <div className="flex items-center gap-2 text-sm p-2 bg-muted rounded">
      {file.type.startsWith("audio/") && <FileAudio className="h-4 w-4" />}
      {file.type.startsWith("video/") && <FileVideo className="h-4 w-4" />}
      {!file.type.startsWith("audio/") && !file.type.startsWith("video/") && (
        <FileText className="h-4 w-4" />
      )}
      <span className="flex-1 truncate">{file.name}</span>
      <span className="text-xs text-muted-foreground">
        {(file.size / 1024 / 1024).toFixed(2)} MB
      </span>
      <Button
        size={"icon"}
        variant={"ghost"}
        onClick={() => controller.remove()}
      >
        <X className="h-4 w-4" />
      </Button>
      {isPending && <span>{progress}%</span>}
    </div>
  );
}
