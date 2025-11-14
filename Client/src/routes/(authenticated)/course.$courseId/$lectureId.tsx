import { createFileRoute } from "@tanstack/react-router";
import { mockLectures } from "@/lib/mock-data";
import { LectureHeader } from "@/components/lectures/lecture-header";
import { MediaPlayer } from "@/components/shared/media-player";
import { LectureTabs } from "@/components/lectures/lecture-tabs"; // Added tabs component
import { useLectureApi } from "@/api/use-lecture-api";
import { FullScreenLoader } from "@/components/shared/loader";

export const Route = createFileRoute(
  "/(authenticated)/course/$courseId/$lectureId"
)({ component: App });

function App() {
  const { lectureId, courseId } = Route.useParams();
  const {
    lecture: { data: lecture, isLoading },
  } = useLectureApi(courseId, lectureId);

  if (isLoading || !lecture) {
    return <FullScreenLoader descriptionText="Loading lecture" />;
  }

  return (
    <>
      <LectureHeader lecture={lecture} courseId={courseId} />
      <div className="container mx-auto px-4 py-8">
        <MediaPlayer lecture={lecture} />

        <div className="mt-8">
          <LectureTabs lecture={lecture} courseId={courseId} />
        </div>
      </div>
    </>
  );
}
