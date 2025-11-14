"use client";

import { useState } from "react";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { BookOpen, FileText, Sparkles, List } from "lucide-react";
import type { Flashcard } from "@/lib/types";
import { LectureDto } from "@/api/web-api-client";
import LectureContentSection from "./content/content-section";

interface LectureTabsProps {
  courseId: string;
  lecture: LectureDto;
}

export function LectureTabs({ lecture, courseId }: LectureTabsProps) {
  const [activeKeyword, setActiveKeyword] = useState<string | null>(null);
  const [activeTab, setActiveTab] = useState("transcript");
  const [selectedFlashcard, setSelectedFlashcard] = useState<Flashcard | null>(
    null
  );

  const handleKeywordClick = (keyword: string) => {
    setActiveKeyword(keyword);
    setActiveTab("transcript");
  };

  return (
    <Tabs value={activeTab} onValueChange={setActiveTab} className="w-full">
      <TabsList className="grid w-full grid-cols-5 mb-8">
        <TabsTrigger value="transcript">
          <FileText className="h-4 w-4 mr-2" /> Transcript
        </TabsTrigger>
        <TabsTrigger value="keywords">
          <BookOpen className="h-4 w-4 mr-2" /> Keywords
        </TabsTrigger>
        <TabsTrigger value="flashcards">
          <Sparkles className="h-4 w-4 mr-2" /> Study Mode
        </TabsTrigger>
        <TabsTrigger
          value="flashcard-list"
          onClick={() => setActiveTab("flashcard-list")}
        >
          <List className="h-4 w-4 mr-2" /> All Flashcards
        </TabsTrigger>
        <TabsTrigger value="content">
          <BookOpen className="h-4 w-4 mr-2" /> Content
        </TabsTrigger>
      </TabsList>

      {
        /* <TabsContent value="transcript">
        <div className="grid grid-cols-1 lg:grid-cols-[1fr_300px] gap-8">
          <TranscriptView lecture={lecture} highlightKeyword={activeKeyword} />
          <KeywordSidebar keywords={lecture.keywords} lectureId={lecture.id} />
        </div>
      </TabsContent>

      <TabsContent value="keywords">
        <div className="max-w-4xl mx-auto">
          <KeywordSidebar keywords={lecture.keywords} lectureId={lecture.id} expanded />
        </div>
      </TabsContent>

      <TabsContent value="flashcards">
        <div className="max-w-3xl mx-auto space-y-6">
          <FlashcardGenerator lecture={lecture} />
          <FlashcardViewer flashcards={lecture.flashcards} onKeywordClick={handleKeywordClick} />
        </div>
      </TabsContent>

      <TabsContent value="flashcard-list">
        <div className="max-w-4xl mx-auto">
          <FlashcardList flashcards={lecture.flashcards} onCardClick={setSelectedFlashcard} />
        </div>
      </TabsContent> */
        <TabsContent value="content">
          <div className=" mx-auto">
            <LectureContentSection contents={lecture.contents} courseId={courseId} lectureId={lecture.id} />
          </div>
        </TabsContent>
      }
    </Tabs>
  );
}
