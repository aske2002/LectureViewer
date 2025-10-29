import { Trip } from "@/types/trip";

export const config = {
  CompanyName: "Sundhedsteknologisk Rejseforening",
  Trips: [
    {
      id: "atlanta-2025",
      destination: {
        name: "Atlanta",
        country: "USA",
      },
      date: new Date("2025-08-01"),
      description: {
        title: "Oplev Atlanta",
        subtitle:
          "En fantast isk mulighed for at opleve den amerikanske kultur og studiemiljø.",
        authors: ["John Doe"],
        authorsOrganisation:
          "2. semesters ingeniørstuderende på Sundhedsteknologi, Institut for elektro- og computerteknologi, Aarhus Universitet",
        sections: [
          {
            title: "Rejsen",
            content:
              "Rejsen til Atlanta er en fantastisk mulighed for at opleve den amerikanske kultur og studiemiljø.",
          },
        ],
      },
      group: { startYear: 2024, startSeason: "Sommer" },
    },
    {
      id: "atlanta-2024",
      destination: {
        name: "Atlanta",
        country: "USA",
      },
      date: new Date("2024-08-01"),
      description: {
        title: "Sundhedsteknologi-studerende på studietur til Georgia, Atlanta",
        subtitle:
          "En studietur gav danske ingeniørstuderende indsigt i den nyeste udvikling inden for medicoteknisk udstyr og forskning.",
        authors: [
          "Anna Victoria H. Bohlbro",
          "Asta Louise Vibe Laursen",
          "Frederik Vestergaard Lausen",
          "Helena Kastbjerg",
          "Lærke Schelde",
          "Rikke Madsen",
          "Signe Holm Quitzau",
        ],
        authorsOrganisation:
          "3. semesters ingeniørstuderende på Sundhedsteknologi, Institut for elektro- og computerteknologi, Aarhus Universitet",
        sections: [
          {
            title: "Underrubrik",
            content: `Som studerende på generalistuddannelsen Sundhedsteknologi, kan det være en udfordring at navigere i hvilke karrieremuligheder, der ligger og venter på os som færdiguddannet. I samarbejde med en lektor på Aarhus Universitet, Peter Johansen, var vi syv studerende fra Aarhus Universitet, som tog på studietur til Georgia, Atlanta i USA, for netop at blive klogere på dette. Her besøgte vi bl.a. virksomhederne OXOS og Veranex samt Georgia Institute of Technology, som er en af de førende forskningsinstitutioner inden for biomedicinsk teknologi og udvikling af medicoteknisk udstyr.  Dette gav os stor indsigt og inspiration til hvordan vores uddannelse kan anvendes i praksis og udvikle innovative sundhedsteknologiske løsninger der kan arbejdes på som færdiguddannede. Studieturen har givet os en unik mulighed for at forstå hvordan sundhedsteknologi anvendes på globalt plan og hvilke trends, der præger branchen.`,
          },
          {
            title: "Fra produkt til marked",
            content: `På turen fik vi et indgående indblik i, hvordan sundhedsteknologi i USA adskiller sig fra vores danske system, og hvordan dette åbner op for nye veje, vi måske ikke tidligere havde overvejet. Den amerikanske godkendelsesproces via FDA (Food and Drug Administration) kan i nogle tilfælde være hurtigere end via EU’s MDR (medical device regulation), hvilket giver et højere tempo i lanceringen af nye medicotekniske løsninger. Dette har også en betydelig indflydelse på, hvordan innovation bringes til live i begge lande. 
På vores studietur blev vi introduceret til "Master of Biomedical Innovation and Development," en kandidatuddannelse i USA, der bygger videre på mange af de emner, vi har beskæftiget os med i nogle af vores fag, såsom Development of Medical Devices, Brugeroplevelsesdesign, m.fl. Denne masteruddannelse giver en endnu dybere forståelse for, hvordan forløbet går fra en idé i udviklingsstadiet og hele vejen til markedet, mens samtidig navigation gennem lovgivning, kliniske tests og markedsføringsgodkendelser er afgørende. Det blev tydeligt, at en sådan uddannelse kunne være en oplagt mulighed for os, der ønsker at udbygge vores bachelor og arbejde i krydsfeltet mellem teknologi, innovation og sundhed. Professor Michael A. Fisher, som er program director for denne kandidatuddannelse, fortæller, at den er meget populær blandt folk der kommer fra industrien og er frustreret over processer i praksis. Derfor vælger de at tage denne kandidat for at få et bedre indblik i fremgangsmåder og få nogle redskaber, så de kan ændre på slagets gang.
Et besøg, der adskilte sig fra de resterende, var hos virksomheden Veranex, som lagde ud med at hente os i en klassisk amerikansk Chevrolet SUV fra hotellet. Veranex er en virksomhed, der specialiserer sig i at lette processen med at få medicotekniske produkter fra idé til marked. Med eksperter inden for alt fra konsulentarbejde og testafdelinger til juridisk rådgivning og regulering, viste Veranex os, hvordan en holistisk tilgang til innovation og udvikling kan være med til at sikre, at nye teknologier hurtigt og sikkert kan komme patienterne til gode. I den sammenhæng fik vi en fremvisning i en nyopført bygning, Science Square, som er dedikeret til entreprenørskab inden for medicoindustrien. For mange af os blev et kontorfællesskab som det sat i perspektiv til innovationsafdelingen InCuba i Aarhus, i forestillingen om hvorvidt vi kan se os selv træde ind i rollen som innovatører.
Vi besøgte også OXOS, en virksomhed der har udviklet håndholdte røntgenscannere, hvilket for alvor illustrerede potentialet i små, bærbare løsninger inden for medicoteknik. Deres teknologi åbner op for en fremtid, hvor patienter kan få adgang til røntgenbilleder på steder og i situationer, hvor det tidligere ikke var muligt, såsom sportsfolk og ved nødsituationer i felten, hvor afstanden til nærmeste hospital ofte er meget stor og derfor et problem.
`,
          },
          {
            title: "USA og Danmark i et større medico-perspektiv",
            content: `USA er generelt førende på rigtig mange områder, herunder inden for forskning og udvikling af medicoteknisk udstyr. Landets store økonomiske kapacitet og privatfinansierede sundhedsvæsen gør det muligt for virksomheder at investere bredt i mange forskellige medicoteknologiske områder, som eksempelvis hjertet og hjertekarsygdomme. Danmark er derimod, på trods af sin langt mindre økonomi, førende inden for specifikke nicheområder såsom høreapparater og diabetesbehandling. Dette skyldes i høj grad en fokuseret prioritering af ressourcerne, da det danske sundhedsvæsen i højere grad er offentligt finansieret.
Under vores uddannelse er vi blevet introduceret til hjertets anatomi og funktionalitet og hvordan man kan anvende teknologiske midler på hjertet gennem fagene Anatomi, Human Physiology og Kardiovaskulær Instrumentering. Studieturen til USA gav os mulighed for at bygge videre på denne viden ved at opleve, hvordan førende eksperter og forskere anvender den nyeste teknologi til at forbedre behandlingen af hjertet og hjertekarsygdomme. Hjertekarsygdomme er en af de største dødsårsager i USA for både mænd og kvinder, og derfor har det amerikanske sundhedssystem, både offentligt og privat, satset massivt på udvikling af innovative løsninger til forebyggelse, diagnostik og behandling af disse lidelser [1]. Den amerikanske medicoteknologiske industri har derfor fokuseret på avancerede teknologier som hjertepumper (LVADs), implanterbare pacemakere, robotassisteret hjertekirurgi, og ikke mindst udviklingen af kunstige hjerter og stamcelleterapier til regenerering af hjertevæv [2]. At tage på studietur til USA og besøge nogle af de førende forskningscentre og virksomheder inden for hjertemedicoteknologi gav os en unik mulighed for at dykke dybt ned og få et førstehåndsindtryk i dette felt. Vi kunne se, hvordan avancerede teknologier, der endnu ikke er tilgængelige i Danmark, bliver udviklet, testet og implementeret. Vi har set eksempler på, hvordan forskning og innovation går hånd i hånd i en sektor, der konstant udvikler sig for at imødekomme de udfordringer, som hjertekarsygdomme præsenterer.`,
          },
        ],
      },
      group: { startYear: 2024, startSeason: "Sommer" },
    },
  ] satisfies Trip[],
};
