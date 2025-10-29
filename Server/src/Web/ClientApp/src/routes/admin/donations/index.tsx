import { createFileRoute } from "@tanstack/react-router";
import { useState } from "react";
import { CalendarIcon, Download, Search } from "lucide-react";
import { format } from "date-fns";
import { Button } from "@/components/ui/button";
import { Calendar } from "@/components/ui/calendar";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { cn } from "@/lib/utils";

export const Route = createFileRoute("/admin/donations/")({
  component: RouteComponent,
});

// Sample data for donations
const donations = [
  {
    id: "DON-001",
    donor: "John Smith",
    email: "john.smith@example.com",
    amount: "$1,000.00",
    date: new Date(2025, 2, 15),
    status: "Completed",
    trip: "Study Abroad - Paris",
  },
  {
    id: "DON-002",
    donor: "Emily Johnson",
    email: "emily.j@example.com",
    amount: "$500.00",
    date: new Date(2025, 2, 14),
    status: "Completed",
    trip: "Exchange Program - Tokyo",
  },
  {
    id: "DON-003",
    donor: "Michael Brown",
    email: "michael.b@example.com",
    amount: "$2,500.00",
    date: new Date(2025, 2, 12),
    status: "Completed",
    trip: "Research Trip - Berlin",
  },
  {
    id: "DON-004",
    donor: "Sarah Davis",
    email: "sarah.d@example.com",
    amount: "$750.00",
    date: new Date(2025, 2, 10),
    status: "Completed",
    trip: "Cultural Exchange - Barcelona",
  },
  {
    id: "DON-005",
    donor: "Robert Wilson",
    email: "robert.w@example.com",
    amount: "$1,200.00",
    date: new Date(2025, 2, 8),
    status: "Completed",
    trip: "Language Program - Rome",
  },
  {
    id: "DON-006",
    donor: "Jennifer Lee",
    email: "jennifer.l@example.com",
    amount: "$300.00",
    date: new Date(2025, 2, 5),
    status: "Completed",
    trip: "Study Abroad - Paris",
  },
  {
    id: "DON-007",
    donor: "David Martinez",
    email: "david.m@example.com",
    amount: "$2,000.00",
    date: new Date(2025, 2, 3),
    status: "Completed",
    trip: "Exchange Program - Tokyo",
  },
  {
    id: "DON-008",
    donor: "Lisa Anderson",
    email: "lisa.a@example.com",
    amount: "$1,500.00",
    date: new Date(2025, 2, 1),
    status: "Completed",
    trip: "Research Trip - Berlin",
  },
];

export default function RouteComponent() {
  const [date, setDate] = useState<Date>();

  return (
    <div className="flex-1 space-y-4 p-4 pt-6 md:p-8">
      <div className="flex items-center justify-between">
        <h2 className="text-3xl font-bold tracking-tight">Donations</h2>
        <Button variant="outline" className="flex items-center gap-1">
          <Download className="h-4 w-4" />
          Export
        </Button>
      </div>

      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">
              Total Donations
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">$9,750.00</div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">
              Average Donation
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">$1,218.75</div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Donors</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">8</div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Trips Funded</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">5</div>
          </CardContent>
        </Card>
      </div>

      <div className="space-y-4">
        <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
          <div className="flex items-center gap-2">
            <Input
              placeholder="Search donations..."
              className="h-9 w-full md:w-[300px]"
            />
            <Button size="sm" variant="ghost">
              <Search className="h-4 w-4" />
              <span className="sr-only">Search</span>
            </Button>
          </div>
          <div className="flex flex-col gap-2 md:flex-row md:items-center">
            <Popover>
              <PopoverTrigger asChild>
                <Button
                  variant={"outline"}
                  className={cn(
                    "w-full justify-start text-left font-normal md:w-[240px]",
                    !date && "text-muted-foreground"
                  )}
                >
                  <CalendarIcon className="mr-2 h-4 w-4" />
                  {date ? format(date, "PPP") : <span>Filter by date</span>}
                </Button>
              </PopoverTrigger>
              <PopoverContent className="w-auto p-0">
                <Calendar
                  mode="single"
                  selected={date}
                  onSelect={setDate}
                  initialFocus
                />
              </PopoverContent>
            </Popover>
            <Select>
              <SelectTrigger className="w-full md:w-[180px]">
                <SelectValue placeholder="Filter by trip" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="all">All Trips</SelectItem>
                <SelectItem value="paris">Study Abroad - Paris</SelectItem>
                <SelectItem value="tokyo">Exchange Program - Tokyo</SelectItem>
                <SelectItem value="berlin">Research Trip - Berlin</SelectItem>
                <SelectItem value="barcelona">
                  Cultural Exchange - Barcelona
                </SelectItem>
                <SelectItem value="rome">Language Program - Rome</SelectItem>
              </SelectContent>
            </Select>
          </div>
        </div>
        <div className="rounded-md border">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Donation ID</TableHead>
                <TableHead>Donor</TableHead>
                <TableHead>Email</TableHead>
                <TableHead>Amount</TableHead>
                <TableHead>Date</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Trip</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {donations.map((donation) => (
                <TableRow key={donation.id}>
                  <TableCell className="font-medium">{donation.id}</TableCell>
                  <TableCell>{donation.donor}</TableCell>
                  <TableCell>{donation.email}</TableCell>
                  <TableCell>{donation.amount}</TableCell>
                  <TableCell>{format(donation.date, "MMM d, yyyy")}</TableCell>
                  <TableCell>{donation.status}</TableCell>
                  <TableCell>{donation.trip}</TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </div>
      </div>
    </div>
  );
}
