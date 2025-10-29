export interface Trip {
  id: string;
  destination: {
    name: string;
    country: string;
  };
  date: Date;
  description: TripDescription;
  group: Group;
}

export interface TripDescription {
  title: string;
  subtitle: string;
  authors: string[];
  authorsOrganisation: string;
  sections: {
    title: string;
    content: string;
  }[];
}

export interface Group {
  startYear: number;
  startSeason: "Vinter" | "Sommer";
}
