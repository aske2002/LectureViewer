import { useState, useRef, useEffect, useCallback, useMemo } from "react";
import {
  Play,
  Pause,
  Volume2,
  VolumeX,
  Maximize2,
  SkipBack,
  SkipForward,
  Download,
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { Slider } from "@/components/ui/slider";
import { FilePreviewHandler } from "@/lib/filePreviews/filePreviewHandler";
import { cn } from "@/lib/utils";

interface MediaPlayerProps {
  url: string;
  name: string;
  type: "audio" | "video";
  allowDownload?: boolean;
  large?: boolean;
}

function MediaPlayer({ url, type, allowDownload = true }: MediaPlayerProps) {
  const mediaRef = useRef<HTMLMediaElement | null>(null);
  const containerRef = useRef<HTMLDivElement | null>(null);
  const controlsTimeoutRef = useRef<NodeJS.Timeout | null>(null);

  const [isPlaying, setIsPlaying] = useState(false);
  const [currentTime, setCurrentTime] = useState(0);
  const [duration, setDuration] = useState(0);
  const [volume, setVolume] = useState(1);
  const [isMuted, setIsMuted] = useState(false);
  const [showControls, setShowControls] = useState(true);

  /** Format time helper **/
  const formatTime = (seconds: number) => {
    if (isNaN(seconds)) return "00:00";
    const m = Math.floor(seconds / 60);
    const s = Math.floor(seconds % 60);
    return `${m.toString().padStart(2, "0")}:${s.toString().padStart(2, "0")}`;
  };

  /** Event handlers **/
  const handleTimeUpdate = useCallback(() => {
    if (mediaRef.current) setCurrentTime(mediaRef.current.currentTime);
  }, []);

  const handleLoadedMetadata = useCallback(() => {
    if (mediaRef.current) setDuration(mediaRef.current.duration || 0);
  }, []);

  const togglePlayPause = useCallback(() => {
    if (!mediaRef.current) return;
    if (isPlaying) {
      mediaRef.current.pause();
      setIsPlaying(false);
    } else {
      const playPromise = mediaRef.current.play();
      if (playPromise !== undefined) {
        playPromise
          .then(() => setIsPlaying(true))
          .catch(() => setIsPlaying(false));
      } else setIsPlaying(true);
    }
  }, [isPlaying]);

  const handleSeek = useCallback((value: number[]) => {
    const newTime = value[0];
    if (mediaRef.current) {
      mediaRef.current.currentTime = newTime;
      setCurrentTime(newTime);
    }
  }, []);

  const handleVolumeChange = useCallback((value: number[]) => {
    const v = value[0];
    if (mediaRef.current) {
      mediaRef.current.volume = v;
      mediaRef.current.muted = v === 0;
    }
    setVolume(v);
    setIsMuted(v === 0);
  }, []);

  const toggleMute = useCallback(() => {
    if (!mediaRef.current) return;
    const newMute = !isMuted;
    mediaRef.current.muted = newMute;
    setIsMuted(newMute);
  }, [isMuted]);

  const skipForward = useCallback(() => {
    if (mediaRef.current) {
      mediaRef.current.currentTime = Math.min(currentTime + 10, duration);
    }
  }, [currentTime, duration]);

  const skipBackward = useCallback(() => {
    if (mediaRef.current) {
      mediaRef.current.currentTime = Math.max(currentTime - 10, 0);
    }
  }, [currentTime]);

  const toggleFullscreen = useCallback(() => {
    if (type !== "video" || !(mediaRef.current instanceof HTMLVideoElement))
      return;
    if (document.fullscreenElement) document.exitFullscreen();
    else mediaRef.current.requestFullscreen();
  }, [type]);

  /** Auto-hide controls on inactivity **/
  const handleMouseMove = useCallback(() => {
    setShowControls(true);
    if (controlsTimeoutRef.current) clearTimeout(controlsTimeoutRef.current);
    if (isPlaying && type === "video") {
      controlsTimeoutRef.current = setTimeout(
        () => setShowControls(false),
        2000
      );
    }
  }, [isPlaying, type]);

  useEffect(() => {
    const handleWindowSpaceKey = (e: KeyboardEvent) => {
      if (
        e.code === "Space" &&
        containerRef.current?.contains(document.activeElement)
      ) {
        e.preventDefault();
        togglePlayPause();
      }
    };
    window.addEventListener("keydown", handleWindowSpaceKey);
    return () => {
      window.removeEventListener("keydown", handleWindowSpaceKey);
    };
  }, [togglePlayPause]);

  useEffect(() => {
    const node = mediaRef.current;
    if (!node) return;
    node.addEventListener("timeupdate", handleTimeUpdate);
    node.addEventListener("loadedmetadata", handleLoadedMetadata);
    return () => {
      node.removeEventListener("timeupdate", handleTimeUpdate);
      node.removeEventListener("loadedmetadata", handleLoadedMetadata);
    };
  }, [handleTimeUpdate, handleLoadedMetadata]);

  useEffect(() => {
    if (mediaRef.current) {
      mediaRef.current.volume = volume;
      mediaRef.current.muted = isMuted;
    }
  }, [volume, isMuted]);

  /** JSX **/
  return (
    <div
      className="relative bg-black rounded-lg overflow-hidden group flex flex-1 h-full w-full"
      ref={containerRef}
      tabIndex={0}
      onMouseMove={handleMouseMove}
    >
      {type === "video" ? (
        <video
          ref={mediaRef as React.RefObject<HTMLVideoElement>}
          src={url}
          className="w-full aspect-video object-cover"
          onClick={togglePlayPause}
          preload="metadata"
        />
      ) : (
        <audio
          ref={mediaRef as React.RefObject<HTMLAudioElement>}
          src={url}
          className="w-full"
          preload="metadata"
          onClick={togglePlayPause}
        />
      )}

      {/* Overlay controls */}
      <div
        className={cn(
          "absolute inset-0 flex flex-col justify-end bg-linear-to-t from-black/80 via-black/30 to-transparenttransition-opacity duration-300",
          { "opacity-100": showControls, "opacity-0": !showControls }
        )}
      >
        {/* Progress bar */}
        <div className="px-3 mb-4">
          <Slider
            value={[currentTime]}
            max={duration || 100}
            step={0.1}
            onValueChange={handleSeek}
            className="w-full"
          />
        </div>

        {/* Main controls */}
        <div className="flex items-center justify-between px-4 pb-3 text-white">
          {/* Left: playback */}
          <div className="flex items-center gap-3">
            <Button variant="ghost" size="sm" onClick={skipBackward}>
              <SkipBack className="h-5 w-5" />
            </Button>
            <Button
              variant="default"
              size="sm"
              onClick={togglePlayPause}
              className="h-10 w-10 rounded-full bg-secondary text-primary-foreground hover:bg-secondary/80"
            >
              {isPlaying ? (
                <Pause className="h-5 w-5" />
              ) : (
                <Play className="h-5 w-5 ml-0.5" />
              )}
            </Button>
            <Button variant="ghost" size="sm" onClick={skipForward}>
              <SkipForward className="h-5 w-5" />
            </Button>

            <span className="text-xs font-mono text-muted-foreground">
              {formatTime(currentTime)} / {formatTime(duration)}
            </span>
          </div>

          {/* Right: volume + fullscreen */}
          <div className="flex items-center gap-3">
            <Button variant="ghost" size="sm" onClick={toggleMute}>
              {isMuted ? (
                <VolumeX className="h-5 w-5" />
              ) : (
                <Volume2 className="h-5 w-5" />
              )}
            </Button>
            <Slider
              value={[isMuted ? 0 : volume]}
              max={1}
              step={0.01}
              onValueChange={handleVolumeChange}
              className="w-20"
            />
            { allowDownload &&
            <a href={url} download className="inline-block">
              <Button variant="ghost" size="sm">
                <Download className="h-5 w-5" />
              </Button>
            </a>
            }
            {type === "video" && (
              <Button variant="ghost" size="sm" onClick={toggleFullscreen}>
                <Maximize2 className="h-5 w-5" />
              </Button>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

/** --- Registry handler --- **/
export const mediaPreviewHandler: FilePreviewHandler = {
  type: "media",
  canHandle: (ext, mime) =>
    ["mp4", "webm", "mov", "mp3", "wav", "ogg"].includes(ext) ||
    mime?.startsWith("audio/") ||
    mime?.startsWith("video/") ||
    false,

  renderInline: ({ url, name, extension, mimeType }) => (
    <MediaPlayer
      url={url}
      name={name}
      type={
        mimeType?.startsWith("audio/") ||
        ["mp3", "wav", "ogg"].includes(extension || "")
          ? "audio"
          : "video"
      }
    />
  ),

  renderModal: ({ url, name, extension, mimeType }) => (
    <MediaPlayer
      url={url}
      name={name}
      type={
        mimeType?.startsWith("audio/") ||
        ["mp3", "wav", "ogg"].includes(extension || "")
          ? "audio"
          : "video"
      }
      large
    />
  ),
};
