import { createFileRoute } from "@tanstack/react-router";
import { mockLectures } from "@/lib/mock-data";
import { LectureHeader } from "@/components/lecture-header";
import { MediaPlayer } from "@/components/media-player";
import { LectureTabs } from "@/components/lecture-tabs"; // Added tabs component

export const Route = createFileRoute("/lecture/$id")({ component: App });

function App() {
  const { id } = Route.useParams();
  const lecture = mockLectures.find((l) => l.id === id);

  if (!lecture) {
    return <div className="container mx-auto px-4 py-8">Lecture not found.</div>;
  }

  return (
    <div className="min-h-screen bg-background">
      <LectureHeader lecture={lecture} />

      <div className="container mx-auto px-4 py-8">
        {lecture.mediaType && <MediaPlayer lecture={lecture} />}

        <div className="mt-8">
          <LectureTabs lecture={lecture} />
        </div>
      </div>
    </div>
  );
}
