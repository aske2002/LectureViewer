import { createFileRoute } from "@tanstack/react-router";
import { BookOpen } from "lucide-react";
import { mockLectures } from "@/lib/mock-data";
import { UploadDialog } from "@/components/upload-dialog";
import { SearchBar } from "@/components/search-bar";
import { LectureCard } from "@/components/lecture-card";

export const Route = createFileRoute("/")({ component: App });

function App() {
  return (
    <div className="min-h-screen bg-background">
      <header className="border-b border-border bg-card">
        <div className="container mx-auto px-4 py-6">
          <div className="flex items-center justify-between mb-6">
            <div className="flex items-center gap-3">
              <img
                src={"/logo512.png"}
                alt="Logo"
                className="h-6  object-contain"
              />
              <div>
                <h1 className="text-2xl font-bold text-balance">
                  Lecture Overview
                </h1>
                <p className="text-sm text-muted-foreground">
                  Transcribe, analyze, and search your lectures
                </p>
              </div>
            </div>
            <UploadDialog />
          </div>

          <SearchBar />
        </div>
      </header>

      <main className="container mx-auto px-4 py-8">
        <div className="mb-6">
          <h2 className="text-xl font-semibold mb-2">All Lectures</h2>
          <p className="text-sm text-muted-foreground">
            {mockLectures.length} lecture{mockLectures.length !== 1 ? "s" : ""}{" "}
            available
          </p>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {mockLectures.map((lecture) => (
            <LectureCard key={lecture.id} lecture={lecture} />
          ))}
        </div>
      </main>
    </div>
  );
}
