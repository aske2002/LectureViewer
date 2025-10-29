import { Badge } from "../ui/badge";
import { Trip as TripType } from "@/types/trip";
import { Route as tripRoute } from "@/routes/(front)/rejser/$tripId";
import LinkButton from "../shared/link-button";

interface UpcomingTripCardProps {
  trip: TripType;
}

export default function Trip({
  trip: { destination, id, date, group },
}: UpcomingTripCardProps) {
  return (
    <div className="group relative overflow-hidden rounded-lg border">
      <div className="absolute inset-0 z-10 bg-black/60 transition-colors group-hover:bg-black/50" />
      <img
        src={`/${id}/card-background.jpeg`}
        alt="Tokyo"
        className="h-60 w-full object-cover transition-transform group-hover:scale-105"
      />
      <div className="absolute inset-0 z-20 flex flex-col justify-end p-6">
        <div className="flex flex-row  justify-start items-center gap-2">
          <h3 className="text-xl font-medium text-white">
            {destination.name}, {destination.country}
          </h3>
          {date > new Date() && <Badge variant="success">Kommende</Badge>}
        </div>
        <p className=" text-md text-white">
          {date
            .toLocaleDateString("da-DK", { month: "long", year: "numeric" })
            .capitalizeFirstLetter()}
        </p>
        <p className="text-sm text-white/80">
          ST {group.startSeason}start {group.startYear}
        </p>
        <LinkButton
          to={tripRoute.to}
          variant={"secondary"}
          params={{
            tripId: id,
          }}
        >
          Læs Mere
        </LinkButton>
      </div>
    </div>
  );
}
