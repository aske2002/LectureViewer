"use client"

import type React from "react"

import { useState } from "react"
import { Button } from "@/components/ui/button"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Upload, FileAudio, FileVideo, FileText } from "lucide-react"

export function UploadDialog() {
  const [open, setOpen] = useState(false)
  const [lectureName, setLectureName] = useState("")
  const [files, setFiles] = useState<File[]>([])

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    // In a real app, this would upload files and process them
    console.log("Uploading:", { lectureName, files })
    setOpen(false)
    setLectureName("")
    setFiles([])
  }

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      setFiles(Array.from(e.target.files))
    }
  }

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button size="lg" className="gap-2">
          <Upload className="h-4 w-4" />
          Upload Lecture
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[500px]">
        <DialogHeader>
          <DialogTitle>Upload New Lecture</DialogTitle>
          <DialogDescription>
            Upload slides, audio, or video recordings. The system will transcribe and analyze the content.
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit} className="space-y-6">
          <div className="space-y-2">
            <Label htmlFor="lecture-name">Lecture Name</Label>
            <Input
              id="lecture-name"
              placeholder="e.g., Introduction to Machine Learning"
              value={lectureName}
              onChange={(e) => setLectureName(e.target.value)}
              required
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="files">Upload Files</Label>
            <div className="border-2 border-dashed border-border rounded-lg p-8 text-center hover:border-primary transition-colors">
              <input
                id="files"
                type="file"
                multiple
                accept="audio/*,video/*,.pdf,.ppt,.pptx"
                onChange={handleFileChange}
                className="hidden"
              />
              <label htmlFor="files" className="cursor-pointer">
                <Upload className="h-10 w-10 mx-auto mb-3 text-muted-foreground" />
                <p className="text-sm font-medium mb-1">Click to upload or drag and drop</p>
                <p className="text-xs text-muted-foreground">Audio, video, slides (PDF, PPT)</p>
              </label>
            </div>
            {files.length > 0 && (
              <div className="mt-4 space-y-2">
                {files.map((file, index) => (
                  <div key={index} className="flex items-center gap-2 text-sm p-2 bg-muted rounded">
                    {file.type.startsWith("audio/") && <FileAudio className="h-4 w-4" />}
                    {file.type.startsWith("video/") && <FileVideo className="h-4 w-4" />}
                    {!file.type.startsWith("audio/") && !file.type.startsWith("video/") && (
                      <FileText className="h-4 w-4" />
                    )}
                    <span className="flex-1 truncate">{file.name}</span>
                    <span className="text-xs text-muted-foreground">{(file.size / 1024 / 1024).toFixed(2)} MB</span>
                  </div>
                ))}
              </div>
            )}
          </div>

          <div className="flex justify-end gap-3">
            <Button type="button" variant="outline" onClick={() => setOpen(false)}>
              Cancel
            </Button>
            <Button type="submit" disabled={!lectureName || files.length === 0}>
              Process Lecture
            </Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  )
}
