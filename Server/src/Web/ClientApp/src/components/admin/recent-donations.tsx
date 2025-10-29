import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"

const recentDonations = [
  {
    id: "DON-001",
    name: "John Smith",
    email: "john.smith@example.com",
    amount: "$1,000.00",
    date: "Mar 15, 2025",
    status: "Completed",
  },
  {
    id: "DON-002",
    name: "Emily Johnson",
    email: "emily.j@example.com",
    amount: "$500.00",
    date: "Mar 14, 2025",
    status: "Completed",
  },
  {
    id: "DON-003",
    name: "Michael Brown",
    email: "michael.b@example.com",
    amount: "$2,500.00",
    date: "Mar 12, 2025",
    status: "Completed",
  },
  {
    id: "DON-004",
    name: "Sarah Davis",
    email: "sarah.d@example.com",
    amount: "$750.00",
    date: "Mar 10, 2025",
    status: "Completed",
  },
  {
    id: "DON-005",
    name: "Robert Wilson",
    email: "robert.w@example.com",
    amount: "$1,200.00",
    date: "Mar 8, 2025",
    status: "Completed",
  },
]

export function RecentDonations() {
  return (
    <div className="space-y-8">
      {recentDonations.map((donation) => (
        <div key={donation.id} className="flex items-center">
          <Avatar className="h-9 w-9">
            <AvatarImage src={`/placeholder.svg?height=36&width=36`} alt={donation.name} />
            <AvatarFallback>{donation.name.charAt(0)}</AvatarFallback>
          </Avatar>
          <div className="ml-4 space-y-1">
            <p className="text-sm font-medium leading-none">{donation.name}</p>
            <p className="text-sm text-muted-foreground">{donation.email}</p>
          </div>
          <div className="ml-auto font-medium">{donation.amount}</div>
        </div>
      ))}
    </div>
  )
}

