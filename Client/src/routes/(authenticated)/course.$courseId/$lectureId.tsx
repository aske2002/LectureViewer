import { createFileRoute } from "@tanstack/react-router";
import { mockLectures } from "@/lib/mock-data";
import { LectureHeader } from "@/components/lecture-header";
import { MediaPlayer } from "@/components/media-player";
import { LectureTabs } from "@/components/lecture-tabs"; // Added tabs component

export const Route = createFileRoute(
  "/(authenticated)/course/$courseId/$lectureId"
)({ component: App });

function App() {
  const { lectureId, courseId } = Route.useParams();
  const lecture = mockLectures.find((l) => l.id === lectureId);

  if (!lecture) {
    return (
      <div className="container mx-auto px-4 py-8">Lecture not found.</div>
    );
  }

  return (
    <>
      <LectureHeader lecture={lecture} courseId={courseId}/>

      <div className="container mx-auto px-4 py-8">
        {lecture.mediaType && <MediaPlayer lecture={lecture} />}

        <div className="mt-8">
          <LectureTabs lecture={lecture} />
        </div>
      </div>
    </>
  );
}
