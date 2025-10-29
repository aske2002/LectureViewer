import { DestinationsClient } from "@/app/web-api-client";
import { useQuery } from "@tanstack/react-query";
import { useMemo } from "react";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../ui/select";
import ResourceImage from "../shared/image";

type CountrySelectorProps ={
  selectedCountry?: string;
  setSelectedCountry: (countryId?: string) => void;
} & React.ComponentProps<typeof Select>;

export default function CountrySelector({
  selectedCountry,
  setSelectedCountry,
}: CountrySelectorProps) {
  const { data: countries } = useQuery({
    queryKey: ["countries"],
    queryFn: () => new DestinationsClient().getCountries(),
  });

  const items = useMemo(() => {
    return countries ? countries : [];
  }, [countries]);

  return (
    <Select
      onValueChange={(value) => {
        setSelectedCountry(value);
      }}
      defaultValue={selectedCountry}
    >
      <SelectTrigger className="w-full">
        <SelectValue placeholder="Vælg land" />
      </SelectTrigger>
      <SelectContent>
        {items.map((item) => (
          <SelectItem key={item.id} value={item.id}>
            <ResourceImage
              src={item.flag}
              alt={item.name}
              width={20}
              height={20}
              className="mr-2 inline-block"
            />
            {item.name}
          </SelectItem>
        ))}
      </SelectContent>
    </Select>
  );
}
