"use client"

import type { Lecture } from "@/lib/types"
import { Card, CardContent } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Slider } from "@/components/ui/slider"
import { Play, Pause, Volume2, VolumeX, Maximize2 } from "lucide-react"
import { useState, useRef, useEffect } from "react"

interface MediaPlayerProps {
  lecture: Lecture
}

export function MediaPlayer({ lecture }: MediaPlayerProps) {
  const [isPlaying, setIsPlaying] = useState(false)
  const [currentTime, setCurrentTime] = useState(0)
  const [duration, setDuration] = useState(0)
  const [volume, setVolume] = useState(1)
  const [isMuted, setIsMuted] = useState(false)
  const videoRef = useRef<HTMLVideoElement>(null)
  const audioRef = useRef<HTMLAudioElement>(null)

  const mediaRef = lecture.mediaType === "video" ? videoRef : audioRef

  useEffect(() => {
    const handleTimeUpdate = () => {
      if (mediaRef.current) {
        setCurrentTime(mediaRef.current.currentTime)
      }
    }

    const handleLoadedMetadata = () => {
      if (mediaRef.current) {
        setDuration(mediaRef.current.duration)
      }
    }

    const media = mediaRef.current
    if (media) {
      media.addEventListener("timeupdate", handleTimeUpdate)
      media.addEventListener("loadedmetadata", handleLoadedMetadata)

      return () => {
        media.removeEventListener("timeupdate", handleTimeUpdate)
        media.removeEventListener("loadedmetadata", handleLoadedMetadata)
      }
    }
  }, [mediaRef])

  useEffect(() => {
    // Listen for keyword navigation to seek to timestamp
    const handleKeywordClick = (event: CustomEvent) => {
      const { timestamp } = event.detail
      if (timestamp && mediaRef.current) {
        const seconds = parseTimestamp(timestamp)
        mediaRef.current.currentTime = seconds
        setCurrentTime(seconds)
      }
    }

    window.addEventListener("keyword-click" as any, handleKeywordClick)
    return () => window.removeEventListener("keyword-click" as any, handleKeywordClick)
  }, [mediaRef])

  const parseTimestamp = (timestamp: string): number => {
    const parts = timestamp.split(":").map(Number)
    if (parts.length === 3) {
      return parts[0] * 3600 + parts[1] * 60 + parts[2]
    }
    return 0
  }

  const formatTime = (seconds: number): string => {
    const hrs = Math.floor(seconds / 3600)
    const mins = Math.floor((seconds % 3600) / 60)
    const secs = Math.floor(seconds % 60)
    return `${hrs.toString().padStart(2, "0")}:${mins.toString().padStart(2, "0")}:${secs.toString().padStart(2, "0")}`
  }

  const togglePlayPause = () => {
    if (mediaRef.current) {
      if (isPlaying) {
        mediaRef.current.pause()
      } else {
        mediaRef.current.play()
      }
      setIsPlaying(!isPlaying)
    }
  }

  const handleSeek = (value: number[]) => {
    if (mediaRef.current) {
      mediaRef.current.currentTime = value[0]
      setCurrentTime(value[0])
    }
  }

  const handleVolumeChange = (value: number[]) => {
    if (mediaRef.current) {
      mediaRef.current.volume = value[0]
      setVolume(value[0])
      setIsMuted(value[0] === 0)
    }
  }

  const toggleMute = () => {
    if (mediaRef.current) {
      mediaRef.current.muted = !isMuted
      setIsMuted(!isMuted)
    }
  }

  const toggleFullscreen = () => {
    if (lecture.mediaType === "video" && videoRef.current) {
      if (document.fullscreenElement) {
        document.exitFullscreen()
      } else {
        videoRef.current.requestFullscreen()
      }
    }
  }

  // Calculate keyword positions on timeline
  const keywordMarkers = lecture.keywords.flatMap((keyword) =>
    keyword.occurrences.map((occurrence) => ({
      term: keyword.term,
      timestamp: occurrence.timestamp,
      position: (parseTimestamp(occurrence.timestamp) / duration) * 100,
    })),
  )

  return (
    <Card className="overflow-hidden">
      <CardContent className="p-0">
        <div className="relative bg-black">
          {lecture.mediaType === "video" ? (
            <video
              ref={videoRef}
              className="w-full aspect-video"
              poster={lecture.thumbnailUrl || "/lecture-video.jpg"}
            >
              <source src={lecture.mediaUrl || ""} type="video/mp4" />
              Your browser does not support the video tag.
            </video>
          ) : (
            <div className="w-full aspect-video flex items-center justify-center bg-gradient-to-br from-primary/20 to-accent/20">
              <audio ref={audioRef}>
                <source src={lecture.mediaUrl || ""} type="audio/mpeg" />
                Your browser does not support the audio tag.
              </audio>
              <div className="text-center">
                <div className="h-24 w-24 rounded-full bg-primary/20 flex items-center justify-center mx-auto mb-4">
                  <Volume2 className="h-12 w-12 text-primary" />
                </div>
                <p className="text-white font-medium">Audio Lecture</p>
              </div>
            </div>
          )}
        </div>

        <div className="p-4 space-y-4 bg-card">
          {/* Timeline with keyword markers */}
          <div className="relative">
            <Slider
              value={[currentTime]}
              max={duration || 100}
              step={0.1}
              onValueChange={handleSeek}
              className="cursor-pointer"
            />
            {/* Keyword markers on timeline */}
            <div className="absolute top-0 left-0 right-0 h-2 pointer-events-none">
              {keywordMarkers.map((marker, index) => (
                <div
                  key={index}
                  className="absolute w-1 h-2 bg-accent rounded-full"
                  style={{ left: `${marker.position}%` }}
                  title={`${marker.term} at ${marker.timestamp}`}
                />
              ))}
            </div>
          </div>

          {/* Controls */}
          <div className="flex items-center justify-between gap-4">
            <div className="flex items-center gap-3">
              <Button variant="ghost" size="sm" onClick={togglePlayPause} className="h-9 w-9 p-0">
                {isPlaying ? <Pause className="h-5 w-5" /> : <Play className="h-5 w-5" />}
              </Button>
              <span className="text-sm font-mono text-muted-foreground">
                {formatTime(currentTime)} / {formatTime(duration)}
              </span>
            </div>

            <div className="flex items-center gap-3">
              <div className="flex items-center gap-2">
                <Button variant="ghost" size="sm" onClick={toggleMute} className="h-9 w-9 p-0">
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

              {lecture.mediaType === "video" && (
                <Button variant="ghost" size="sm" onClick={toggleFullscreen} className="h-9 w-9 p-0">
                  <Maximize2 className="h-4 w-4" />
                </Button>
              )}
            </div>
          </div>
        </div>
      </CardContent>
    </Card>
  )
}
