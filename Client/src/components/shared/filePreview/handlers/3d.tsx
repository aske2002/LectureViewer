import React, {
  Suspense,
  useCallback,
  useEffect,
  useMemo,
  useRef,
  useState,
} from "react";
import { Canvas } from "@react-three/fiber";
import {
  OrbitControls,
  Environment,
  Bounds,
  Center,
  useGLTF,
  Html,
  useProgress,
  Grid,
} from "@react-three/drei";
import * as THREE from "three";
import { Button } from "@/components/ui/button";
import { Slider } from "@/components/ui/slider";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Card } from "@/components/ui/card";
import { Toggle } from "@/components/ui/toggle";
import {
  Maximize2,
  RotateCcw,
  SunMedium,
  Move,
  Layers,
  Grid2X2,
  Loader2,
  LucideRotate3D,
} from "lucide-react";
import { FilePreviewHandler } from "@/lib/filePreviews/filePreviewHandler";
import {
  presetsObj,
  PresetsType,
} from "@react-three/drei/helpers/environment-assets";
import { cn } from "@/lib/utils";
import { Spinner } from "@/components/ui/spinner";

/** ---------- Small helpers ---------- **/

function LoadingOverlay() {
  const { active, progress } = useProgress();
  if (!active) return null;
  return (
    <Html center>
      <div className="flex items-center gap-2 rounded-md bg-black/70 text-white px-3 py-2 text-sm">
        <Spinner className="h-5 w-5" />
        <span>Loading {progress.toFixed(0)}%</span>
      </div>
    </Html>
  );
}

function ErrorFallback({ message }: { message: string }) {
  return (
    <div className="flex items-center justify-center h-full">
      <div className="rounded-md border bg-muted/40 px-4 py-3 text-sm text-destructive">
        {message}
      </div>
    </div>
  );
}

/** Load via Blob URL to avoid “octet-stream” / header issues */
async function getObjectUrl(src: string): Promise<string> {
  const res = await fetch(src);
  if (!res.ok) throw new Error(`Failed to fetch model (${res.status})`);
  const blob = await res.blob();
  return URL.createObjectURL(blob);
}

/** ---------- Core 3D components ---------- **/

const ENV_PRESETS = Object.keys(presetsObj) as (keyof typeof presetsObj)[];

function applyWireframe(obj: THREE.Object3D, wireframe: boolean) {
  obj.traverse((child) => {
    const mesh = child as THREE.Mesh;
    if (mesh.isMesh) {
      const mat = mesh.material as THREE.Material | THREE.Material[];
      if (Array.isArray(mat)) {
        mat.forEach((m) => {
          const mm = m as THREE.MeshStandardMaterial;
          if ("wireframe" in mm) mm.wireframe = wireframe;
        });
      } else if (mat) {
        const mm = mat as THREE.MeshStandardMaterial;
        if ("wireframe" in mm) mm.wireframe = wireframe;
      }
    }
  });
}

function Model({
  url,
  onLoaded,
  wireframe,
}: {
  url: string;
  onLoaded?: (root: THREE.Object3D) => void;
  wireframe: boolean;
}) {
  const gltf = useGLTF(url);
  const groupRef = useRef<THREE.Group>(null);

  useEffect(() => {
    if (groupRef.current) {
      applyWireframe(groupRef.current, wireframe);
    }
  }, [wireframe]);

  useEffect(() => {
    if (groupRef.current && onLoaded) onLoaded(groupRef.current);
  }, [onLoaded]);

  return (
    <Center disableY>
      <group ref={groupRef}>
        <primitive object={gltf.scene} />
      </group>
    </Center>
  );
}
useGLTF.preload?.(""); // no-op safeguard

/** ---------- Viewer shell with UI ---------- **/

function GlbViewer({ src, large }: { src: string; large?: boolean }) {
  const [objectUrl, setObjectUrl] = useState<string | null>(null);
  const [err, setErr] = useState<string | null>(null);

  const [env, setEnv] = useState<PresetsType>("studio");
  const [grid, setGrid] = useState(true);
  const [wireframe, setWireframe] = useState(false);
  const [bg, setBg] = useState<"dark" | "light" | "transparent">("dark");
  const [rotation, setRotation] = useState(0); // Y rotation (manual)
  const [autoRotate, setAutoRotate] = useState(true);

  const bgClass =
    bg === "dark"
      ? "bg-[#0b0b0c]"
      : bg === "light"
        ? "bg-white"
        : "bg-transparent";

  useEffect(() => {
    let revoked = false;
    (async () => {
      try {
        const url = await getObjectUrl(src);
        if (!revoked) setObjectUrl(url);
      } catch (e: any) {
        setErr(e?.message ?? "Failed to open 3D model.");
      }
    })();
    return () => {
      revoked = true;
      if (objectUrl) URL.revokeObjectURL(objectUrl);
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [src]);

  const onFitView = useCallback(() => {
    // trigger <Bounds> to fit current scene via key change
    setRotation(0);
  }, []);

  const overlay = (
    <div className="absolute inset-x-0 bottom-0 p-3">
      <div className="flex items-center justify-between gap-3 rounded-xl bg-black/45 px-3 py-2 backdrop-blur">
        {/* Left controls */}
        <div className="flex items-center gap-2">
          {/* Move/fit */}
          <Button
            size="sm"
            variant="ghost"
            className="text-white"
            onClick={onFitView}
            title="Fit to view"
          >
            <Move className="h-4 w-4" />
          </Button>

          {/* Reset rot */}
          <Button
            size="sm"
            variant="ghost"
            className="text-white"
            onClick={() => setRotation(0)}
            title="Reset rotation"
          >
            <RotateCcw className="h-4 w-4" />
          </Button>

          {/* Manual rotation (Y) */}
          <div className="hidden md:flex items-center gap-2 text-white">
            <span className="text-xs opacity-80">Rotate</span>
            <Slider
              value={[rotation]}
              min={-Math.PI}
              max={Math.PI}
              step={0.01}
              onValueChange={(v) => setRotation(v[0])}
              className="w-28"
            />
          </div>

          {/* Wireframe */}
          <Toggle
            pressed={wireframe}
            onPressedChange={setWireframe}
            className="text-white data-[state=on]:bg-primary-foreground"
            aria-label="Toggle wireframe"
          >
            <Layers className="h-4 w-4" />
          </Toggle>

          {/* Grid */}
          <Toggle
            pressed={grid}
            onPressedChange={setGrid}
            className="text-white data-[state=on]:bg-primary-foreground"
            aria-label="Toggle grid"
          >
            <Grid2X2 className="h-4 w-4" />
          </Toggle>

          {/* Auto rotate */}
          <Toggle
            pressed={autoRotate}
            onPressedChange={setAutoRotate}
            title="Toggle auto-rotate"
            className="text-white data-[state=on]:bg-primary-foreground"
            aria-label="Toggle auto-rotate"
          >
            <LucideRotate3D
              className={`h-4 w-4 ${autoRotate ? "animate-spin-slow" : ""}`}
            />
          </Toggle>
        </div>

        {/* Right controls */}
        <div className="flex items-center gap-2">
          {/* Environment */}
          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <Button size="sm" variant={"secondary"} className="gap-2">
                <SunMedium className="h-4 w-4" />
                {env}
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent>
              {ENV_PRESETS.map((p) => (
                <DropdownMenuItem key={p} onClick={() => setEnv(p)}>
                  {p}
                </DropdownMenuItem>
              ))}
            </DropdownMenuContent>
          </DropdownMenu>

          {/* Background */}
          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <Button size="sm" variant="secondary">
                Background
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent>
              <DropdownMenuItem onClick={() => setBg("dark")}>
                Dark
              </DropdownMenuItem>
              <DropdownMenuItem onClick={() => setBg("light")}>
                Light
              </DropdownMenuItem>
              <DropdownMenuItem onClick={() => setBg("transparent")}>
                Transparent
              </DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
        </div>
      </div>
    </div>
  );

  if (err) return <ErrorFallback message={err} />;
  if (!objectUrl)
    return (
      <div
        className={cn("relative flex items-center justify-center h-full w-full", bgClass)}
      >
        <div className="absolute inset-0 flex items-center justify-center">
          <div className="flex items-center gap-2 text-white/80">
            <Spinner className="h-5 w-5" />
            <span>Preparing 3D viewer…</span>
          </div>
        </div>
      </div>
    );

  return (
    <Card
      className={cn("relative overflow-hidden border-none h-full flex flex-1", bgClass)}
    >
      <Canvas
        dpr={[1, 2]}
        camera={{ position: [2, 1.5, 2.5], fov: 45, near: 0.1, far: 1000 }}
      >
        <Suspense fallback={<LoadingOverlay />}>
          {/* Lighting / environment */}
          <Environment preset={env} />

          {/* Fit-to-view and clip to bounds */}
          <Bounds fit clip observe margin={1.2}>
            {/* Rotate wrapper */}
            <group rotation={[0, rotation, 0]}>
              <Model
                url={objectUrl}
                wireframe={wireframe}
                onLoaded={() => {
                  // Bounds will auto-fit on mount thanks to <Bounds observe />
                }}
              />
            </group>
          </Bounds>

          {/* Grid floor */}
          {grid && (
            <Grid
              args={[10, 10]}
              cellSize={0.5}
              cellThickness={0.75}
              sectionSize={2.5}
              sectionThickness={1.5}
              sectionColor={"#808080"}
              fadeDistance={30}
              fadeStrength={2}
              infiniteGrid
            />
          )}

          <OrbitControls
            makeDefault
            enableDamping
            dampingFactor={0.07}
            autoRotate={autoRotate}
            autoRotateSpeed={0.5}
          />
        </Suspense>
      </Canvas>

      {/* Overlay UI */}
      {overlay}
    </Card>
  );
}

/** ---------- Registry handler ---------- **/

export const model3dPreviewHandler: FilePreviewHandler = {
  type: "model3d",
  canHandle: (ext, mime) =>
    ext === "glb" ||
    ext === "gltf" ||
    mime === "model/gltf-binary" ||
    mime === "model/gltf+json",

  renderInline: ({ url }) => <GlbViewer src={url} />,
  renderModal: ({ url }) => <GlbViewer src={url} large />,
};
