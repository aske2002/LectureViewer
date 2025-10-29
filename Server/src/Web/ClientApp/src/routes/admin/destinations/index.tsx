import { createFileRoute } from "@tanstack/react-router";
import { useMemo, useState } from "react";
import { PlusCircle } from "lucide-react";

import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  Form,
  FormMessage,
} from "@/components/ui/form";
import {
  CreateDestinationCommand,
  DestinationDto,
  DestinationsClient,
} from "@/app/web-api-client";
import { createColumnHelper, getCoreRowModel } from "@tanstack/react-table";
import { DataTable } from "@/components/data-table";
import { useMutation, useQuery } from "@tanstack/react-query";
import CountrySelector from "@/components/admin/country-selector";
import { LoadingButton } from "@/components/shared/loading-button";
import ResourceImage from "@/components/shared/image";

export const Route = createFileRoute("/admin/destinations/")({
  component: RouteComponent,
});

const columnHelper = createColumnHelper<DestinationDto>();

export const columns = [
  columnHelper.accessor("id", {
    header: "Id",
    cell: (props) => props.getValue(),
  }),
  columnHelper.accessor("name", {
    header: "Lokation",
    cell: (props) => props.getValue(),
  }),
  columnHelper.accessor("country", {
    header: "Land",
    cell: props => 
      <div className="flex items-center gap-2">
        <ResourceImage
          src={props.getValue().flag}
          alt="Country"
          width={40}
          height={20}
          className="rounded-sm"
        />
        {props.getValue().name}
      </div>
    ,
  }),
  columnHelper.accessor("description", {
    header: "Beskrivelse",
    cell: (props) => props.getValue(),
  }),
];

export default function RouteComponent() {
  const [open, setOpen] = useState(false);

  const { data: destinations } = useQuery({
    queryKey: ["destinations"],
    queryFn: () => new DestinationsClient().getDestinations(),
  });

  const items = useMemo(() => {
    return destinations ? destinations.items : [];
  }, [destinations]);

  return (
    <div className="flex-1 space-y-4 p-4 pt-6 md:p-8">
      <div className="flex items-center justify-between">
        <h2 className="text-3xl font-bold tracking-tight">Destinationer</h2>
        <Button
          className="flex items-center gap-1"
          onClick={() => setOpen(true)}
        >
          <PlusCircle className="h-4 w-4" />
          Ny destination
        </Button>
      </div>
      <CreateDestination open={open} setOpen={setOpen} />

      <div className="rounded-md border">
        <DataTable columns={columns} data={items} getCoreRowModel={getCoreRowModel()}/>
      </div>
    </div>
  );
}

const createDestinationSchema = z.object({
  name: z.string().min(1, { message: "Navn er påkrævet" }),
  countryId: z.string().min(1, { message: "Land er påkrævet" }),
  description: z.string().optional(),
});

interface CreateDestinationProps {
  open: boolean;
  setOpen: (open: boolean) => void;
}

const CreateDestination = ({ open, setOpen }: CreateDestinationProps) => {
  const form = useForm({
    resolver: zodResolver(createDestinationSchema),
    defaultValues: {
      name: "",
      countryId: "",
      description: "",
    },
  });

  const { mutateAsync: createDestination, isPending } = useMutation({
    mutationFn: (data: CreateDestinationCommand) =>
      new DestinationsClient().createDestination(
        new CreateDestinationCommand(data)
      ),
  });

  const onSubmit = async (data: z.infer<typeof createDestinationSchema>) => {
    await createDestination(
      new CreateDestinationCommand({
        name: data.name,
        countryId: data.countryId,
        description: data.description,
      })
    );
  };

  return (
    <Form {...form}>
      <form className="space-y-4" onSubmit={form.handleSubmit(onSubmit)}>
        <Dialog open={open} onOpenChange={setOpen}>
          <DialogContent className="sm:max-w-[600px]">
            <DialogHeader>
              <DialogTitle>Opret destination</DialogTitle>
              <DialogDescription>
                Udfyld oplysningerne nedenfor for at oprette en ny destination.
              </DialogDescription>
            </DialogHeader>
            <div className="flex flex-col gap-6">
              <FormField
                control={form.control}
                name="name"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Lokation</FormLabel>
                    <FormControl>
                      <Input {...field} placeholder="By, område e.g" />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="countryId"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Land</FormLabel>
                    <FormControl>
                      <CountrySelector
                        selectedCountry={field.value}
                        setSelectedCountry={(countryId) => {
                          field.onChange(countryId);
                        }}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="description"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Beskrivelse</FormLabel>
                    <FormControl>
                      <Textarea {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>
            <DialogFooter>
              <LoadingButton
                loading={isPending}
                type="submit"
                onClick={form.handleSubmit(onSubmit)}
              >
                Opret
              </LoadingButton>
            </DialogFooter>
          </DialogContent>
        </Dialog>
      </form>
    </Form>
  );
};
