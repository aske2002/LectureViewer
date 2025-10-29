import { Badge } from "@/components/ui/badge";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";

const upcomingTrips = [
  {
    id: "TRIP-001",
    name: "Study Abroad - Paris",
    destination: "Paris, France",
    startDate: "Jun 15, 2025",
    endDate: "Sep 30, 2025",
    students: 8,
    status: "Upcoming",
  },
  {
    id: "TRIP-002",
    name: "Exchange Program - Tokyo",
    destination: "Tokyo, Japan",
    startDate: "Jul 1, 2025",
    endDate: "Dec 15, 2025",
    students: 5,
    status: "Upcoming",
  },
  {
    id: "TRIP-003",
    name: "Research Trip - Berlin",
    destination: "Berlin, Germany",
    startDate: "Aug 10, 2025",
    endDate: "Sep 25, 2025",
    students: 6,
    status: "Upcoming",
  },
  {
    id: "TRIP-004",
    name: "Cultural Exchange - Barcelona",
    destination: "Barcelona, Spain",
    startDate: "May 20, 2025",
    endDate: "Jul 10, 2025",
    students: 10,
    status: "Upcoming",
  },
  {
    id: "TRIP-005",
    name: "Language Program - Rome",
    destination: "Rome, Italy",
    startDate: "Jun 5, 2025",
    endDate: "Aug 20, 2025",
    students: 7,
    status: "Upcoming",
  },
];

export function UpcomingTrips() {
  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead>Trip</TableHead>
          <TableHead>Destination</TableHead>
          <TableHead>Start Date</TableHead>
          <TableHead>End Date</TableHead>
          <TableHead>Students</TableHead>
          <TableHead>Status</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {upcomingTrips.map((trip) => (
          <TableRow key={trip.id}>
            <TableCell className="font-medium">{trip.name}</TableCell>
            <TableCell>{trip.destination}</TableCell>
            <TableCell>{trip.startDate}</TableCell>
            <TableCell>{trip.endDate}</TableCell>
            <TableCell>{trip.students}</TableCell>
            <TableCell>
              <Badge
                variant="outline"
                className="bg-blue-50 text-blue-700 hover:bg-blue-50"
              >
                {trip.status}
              </Badge>
            </TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  );
}
