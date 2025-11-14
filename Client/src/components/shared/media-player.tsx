import { useState, useRef, useEffect, useMemo, useCallback } from "react";
import {
  Play,
  Pause,
  Volume2,
  VolumeX,
  Maximize2,
  Minimize,
  SkipBack,
  SkipForward,
  X,
} from "lucide-react";
import { Card } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Slider } from "@/components/ui/slider";
import { Tooltip, TooltipContent, TooltipTrigger } from "@/components/ui/tooltip";
import { LectureContentType, LectureDto } from "@/api/web-api-client";

interface MediaPlayerProps {
  lecture: LectureDto;
}

export function MediaPlayer({ lecture }: MediaPlayerProps) {
  const videoRef = useRef<HTMLVideoElement>(null);
  const audioRef = useRef<HTMLAudioElement>(null);
  const controlsTimeoutRef = useRef<NodeJS.Timeout | null>(null);

  const [isPlaying, setIsPlaying] = useState(false);
  const [currentTime, setCurrentTime] = useState(0);
  const [duration, setDuration] = useState(0);
  const [volume, setVolume] = useState(1);
  const [isMuted, setIsMuted] = useState(false);
  const [showControls, setShowControls] = useState(true);
  const [isFloating, setIsFloating] = useState(false);

  // Select which media ref is active (video or audio)
  const mediaRef = useMemo(() => {
    const type = lecture.primaryContent?.contentType;
    if (type === LectureContentType.Video) return videoRef;
    if (type === LectureContentType.Audio) return audioRef;
    return videoRef; // fallback
  }, [lecture.primaryContent?.contentType]);

  /** --- Helpers --- **/
  const formatTime = (seconds: number) => {
    if (isNaN(seconds)) return "00:00";
    const h = Math.floor(seconds / 3600);
    const m = Math.floor((seconds % 3600) / 60);
    const s = Math.floor(seconds % 60);
    return `${h > 0 ? `${h.toString().padStart(2, "0")}:` : ""}${m
      .toString()
      .padStart(2, "0")}:${s.toString().padStart(2, "0")}`;
  };

  /** --- Media Event Handling --- **/
  const handleTimeUpdate = useCallback(() => {
    if (mediaRef.current) setCurrentTime(mediaRef.current.currentTime);
  }, [mediaRef]);

  const handleLoadedMetadata = useCallback(() => {
    if (mediaRef.current) setDuration(mediaRef.current.duration || 0);
  }, [mediaRef]);

  /** --- Core Controls --- **/
  const togglePlayPause = useCallback(() => {
    if (!mediaRef.current) return;
    if (isPlaying) {
      mediaRef.current.pause();
      setIsPlaying(false);
    } else {
      const playPromise = mediaRef.current.play();
      if (playPromise !== undefined) {
        playPromise.then(() => setIsPlaying(true)).catch(() => setIsPlaying(false));
      } else {
        setIsPlaying(true);
      }
    }
  }, [isPlaying, mediaRef]);

  const handleSeek = useCallback(
    (value: number[]) => {
      const newTime = value[0];
      if (mediaRef.current) {
        mediaRef.current.currentTime = newTime;
        setCurrentTime(newTime);
      }
    },
    [mediaRef]
  );

  const handleVolumeChange = useCallback(
    (value: number[]) => {
      const v = value[0];
      if (mediaRef.current) {
        mediaRef.current.volume = v;
        mediaRef.current.muted = v === 0;
      }
      setVolume(v);
      setIsMuted(v === 0);
    },
    [mediaRef]
  );

  const toggleMute = useCallback(() => {
    if (!mediaRef.current) return;
    const newMute = !isMuted;
    mediaRef.current.muted = newMute;
    setIsMuted(newMute);
  }, [isMuted, mediaRef]);

  const skipForward = useCallback(() => {
    if (mediaRef.current) {
      mediaRef.current.currentTime = Math.min(currentTime + 10, duration);
    }
  }, [currentTime, duration, mediaRef]);

  const skipBackward = useCallback(() => {
    if (mediaRef.current) {
      mediaRef.current.currentTime = Math.max(currentTime - 10, 0);
    }
  }, [currentTime, mediaRef]);

  const toggleFullscreen = useCallback(() => {
    if (lecture.primaryContent?.contentType !== LectureContentType.Video || !videoRef.current) return;
    if (document.fullscreenElement) document.exitFullscreen();
    else videoRef.current.requestFullscreen();
  }, [lecture.primaryContent?.contentType]);

  const toggleFloating = () => {
    setIsFloating((prev) => !prev);
    setShowControls(true);
  };

  /** --- Show/hide controls --- **/
  const handleMouseMove = useCallback(() => {
    setShowControls(true);
    if (controlsTimeoutRef.current) clearTimeout(controlsTimeoutRef.current);
    if (
      isPlaying &&
      lecture.primaryContent?.contentType === LectureContentType.Video &&
      !isFloating
    ) {
      controlsTimeoutRef.current = setTimeout(() => setShowControls(false), 2000);
    }
  }, [isPlaying, isFloating, lecture.primaryContent?.contentType]);

  /** --- Attach listeners dynamically via callback ref --- **/
  const attachMediaRef = useCallback(
    (node: HTMLMediaElement | null) => {
      if (!node) return;

      // Assign correct ref
      if (lecture.primaryContent?.contentType === LectureContentType.Video)
        videoRef.current = node as HTMLVideoElement;
      else if (lecture.primaryContent?.contentType === LectureContentType.Audio)
        audioRef.current = node as HTMLAudioElement;

      // Restore state
      node.currentTime = currentTime;
      node.volume = volume;
      node.muted = isMuted;

      // Attach listeners
      node.addEventListener("timeupdate", handleTimeUpdate);
      node.addEventListener("loadedmetadata", handleLoadedMetadata);

      // Cleanup on unmount
      return () => {
        node.removeEventListener("timeupdate", handleTimeUpdate);
        node.removeEventListener("loadedmetadata", handleLoadedMetadata);
      };
    },
    [lecture.primaryContent?.contentType, currentTime, volume, isMuted, handleTimeUpdate, handleLoadedMetadata]
  );

  /** --- Keep volume/mute in sync --- **/
  useEffect(() => {
    if (mediaRef.current) {
      mediaRef.current.volume = volume;
      mediaRef.current.muted = isMuted;
    }
  }, [volume, isMuted, mediaRef]);

  /** --- Media JSX --- **/
  const MediaElement = () => {
    const src = lecture.primaryContent?.url || "";
    if (lecture.primaryContent?.contentType === LectureContentType.Video) {
      return (
        <video
          ref={attachMediaRef}
          src={src}
          className="w-full aspect-video object-cover"
          onClick={togglePlayPause}
          onDoubleClick={toggleFullscreen}
          preload="metadata"
        />
      );
    }
    if (lecture.primaryContent?.contentType === LectureContentType.Audio) {
      return <audio ref={attachMediaRef} src={src} preload="metadata" />;
    }
    return null;
  };

  /** --- Floating Player --- **/
  if (isFloating) {
    return (
      <div className="fixed bottom-6 right-6 z-50 w-96 shadow-xl rounded-lg overflow-hidden border bg-card">
        <div className="relative bg-black" onMouseMove={handleMouseMove}>
          {MediaElement()}
          {!isPlaying && (
            <div className="absolute inset-0 flex items-center justify-center bg-black/30">
              <Button onClick={togglePlayPause} className="h-14 w-14 rounded-full bg-primary/90 hover:bg-primary">
                <Play className="h-7 w-7 text-primary-foreground ml-0.5" />
              </Button>
            </div>
          )}
          <div
            className={`absolute top-2 right-2 flex gap-2 transition-opacity ${
              showControls ? "opacity-100" : "opacity-0"
            }`}
          >
            <Button
              size="sm"
              onClick={toggleFloating}
              className="h-8 w-8 p-0 bg-black/60 hover:bg-black/80 text-white rounded-full"
              title="Dock player"
            >
              <Maximize2 className="h-4 w-4" />
            </Button>
            <Button
              size="sm"
              onClick={() => setIsFloating(false)}
              className="h-8 w-8 p-0 bg-black/60 hover:bg-black/80 text-white rounded-full"
              title="Close"
            >
              <X className="h-4 w-4" />
            </Button>
          </div>
        </div>

        <div className="p-3 space-y-2 bg-card">
          <Slider
            value={[currentTime]}
            max={duration || 100}
            step={0.1}
            onValueChange={handleSeek}
          />

          <div className="flex items-center justify-between">
            <div className="flex items-center gap-1">
              <Button variant="ghost" size="sm" onClick={skipBackward}>
                <SkipBack className="h-4 w-4" />
              </Button>
              <Button variant="default" size="sm" onClick={togglePlayPause}>
                {isPlaying ? <Pause className="h-4 w-4" /> : <Play className="h-4 w-4 ml-0.5" />}
              </Button>
              <Button variant="ghost" size="sm" onClick={skipForward}>
                <SkipForward className="h-4 w-4" />
              </Button>
            </div>

            <div className="flex items-center gap-2">
              <Tooltip>
                <TooltipTrigger asChild>
                  <Button variant="ghost" size="sm" onClick={toggleMute}>
                    {isMuted ? <VolumeX className="h-4 w-4" /> : <Volume2 className="h-4 w-4" />}
                  </Button>
                </TooltipTrigger>
                <TooltipContent>
                  <Slider
                    orientation="vertical"
                    value={[isMuted ? 0 : volume * 100]}
                    max={100}
                    step={1}
                    onValueChange={(v) => handleVolumeChange([v[0] / 100])}
                  />
                </TooltipContent>
              </Tooltip>
              <span className="text-xs font-mono text-muted-foreground">
                {formatTime(currentTime)}
              </span>
            </div>
          </div>
        </div>
      </div>
    );
  }

  /** --- Docked Player --- **/
  return (
    <Card className="overflow-hidden shadow-lg bg-card">
      <div className="relative bg-black" onMouseMove={handleMouseMove}>
        {MediaElement()}
        {!isPlaying && (
          <div className="absolute inset-0 flex items-center justify-center bg-black/20">
            <Button
              onClick={togglePlayPause}
              className="h-20 w-20 rounded-full bg-primary/90 hover:bg-primary transition-transform"
            >
              <Play className="h-10 w-10 text-primary-foreground ml-1" />
            </Button>
          </div>
        )}
      </div>

      <div className="p-4 bg-card space-y-4">
        <Slider value={[currentTime]} max={duration || 100} step={0.1} onValueChange={handleSeek} />
        <div className="flex justify-between text-xs font-mono text-muted-foreground">
          <span>{formatTime(currentTime)}</span>
          <span>{formatTime(duration)}</span>
        </div>

        <div className="flex items-center justify-between gap-4">
          <div className="flex items-center gap-2">
            <Button variant="ghost" size="sm" onClick={skipBackward}>
              <SkipBack className="h-5 w-5" />
            </Button>
            <Button variant="default" size="sm" onClick={togglePlayPause} className="h-12 w-12 rounded-full">
              {isPlaying ? <Pause className="h-6 w-6" /> : <Play className="h-6 w-6 ml-0.5" />}
            </Button>
            <Button variant="ghost" size="sm" onClick={skipForward}>
              <SkipForward className="h-5 w-5" />
            </Button>
          </div>

          <div className="flex items-center gap-4">
            <div className="flex items-center gap-2 bg-muted/50 rounded-full px-3 py-2">
              <Button variant="ghost" size="sm" onClick={toggleMute}>
                {isMuted ? <VolumeX className="h-4 w-4" /> : <Volume2 className="h-4 w-4" />}
              </Button>
              <Slider
                value={[isMuted ? 0 : volume]}
                max={1}
                step={0.01}
                onValueChange={handleVolumeChange}
                className="w-24"
              />
            </div>

            <Button variant="ghost" size="sm" onClick={toggleFloating}>
              <Minimize className="h-4 w-4" />
            </Button>

            {lecture.primaryContent?.contentType === LectureContentType.Video && (
              <Button variant="ghost" size="sm" onClick={toggleFullscreen}>
                <Maximize2 className="h-4 w-4" />
              </Button>
            )}
          </div>
        </div>
      </div>
    </Card>
  );
}
