import { createFileRoute } from "@tanstack/react-router";
import { config } from "@/lib/config";
import DonateButton from "@/components/shared/donate-button";

export const Route = createFileRoute("/(front)/om-os/")({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <div className="flex flex-col min-h-screen">
      <main className="flex-1">
        <section className="w-full py-12 md:py-24 lg:py-32 bg-gradient-to-r from-blue-50 to-indigo-50">
          <div className="container px-4 md:px-6 mx-auto">
            <div className="grid gap-6 lg:grid-cols-2 lg:gap-12 items-center">
              <div className="space-y-4">
                <h1 className="text-3xl font-bold tracking-tighter sm:text-4xl md:text-5xl">
                  Om {config.CompanyName}
                </h1>
                <p className="max-w-[600px] text-gray-500 md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                  Lær mere om vores forening, vores mission og de mennesker, der
                  gør det muligt.
                </p>
              </div>
              <img
                src="/placeholder.svg?height=550&width=800"
                alt="{config.CompanyName} team"
                className="mx-auto aspect-video overflow-hidden rounded-xl object-cover object-center sm:w-full"
              />
            </div>
          </div>
        </section>
        <section className="w-full py-12 md:py-24 lg:py-32">
          <div className="container px-4 md:px-6 mx-auto">
            <div className="grid gap-6 lg:grid-cols-2 lg:gap-12">
              <div className="space-y-4">
                <h2 className="text-3xl font-bold tracking-tighter">
                  Vores Historie
                </h2>
                <p className="text-gray-500 md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                  {config.CompanyName} blev grundlagt i 2018 af en gruppe
                  studerende fra Københavns Universitet med en passion for
                  rejser og kulturel udveksling. Det begyndte som en lille
                  studiegruppe, der ønskede at udforske uddannelsessystemer i
                  andre lande.
                </p>
                <p className="text-gray-500 md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                  Efter vores første succesfulde tur til Berlin i 2018, voksede
                  interessen blandt andre studerende, og vi besluttede at
                  etablere en officiel forening. Siden da har vi arrangeret over
                  15 {config.CompanyName} til forskellige lande i Europa og
                  Asien.
                </p>
              </div>
              <div className="space-y-4">
                <h2 className="text-3xl font-bold tracking-tighter">
                  Vores Mission
                </h2>
                <p className="text-gray-500 md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                  Vores mission er at give studerende mulighed for at opleve
                  forskellige uddannelsessystemer, kulturer og traditioner,
                  samtidig med at de knytter værdifulde kontakter med
                  internationale studerende.
                </p>
                <p className="text-gray-500 md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                  Vi tror på, at international erfaring er afgørende for
                  personlig og faglig udvikling, og vi stræber efter at gøre
                  disse oplevelser tilgængelige for så mange studerende som
                  muligt, uanset deres økonomiske baggrund.
                </p>
              </div>
            </div>
          </div>
        </section>
        <section className="w-full py-12 md:py-24 lg:py-32 bg-gray-100">
          <div className="container px-4 md:px-6 mx-auto">
            <div className="flex flex-col items-center justify-center space-y-4 text-center">
              <div className="space-y-2">
                <h2 className="text-3xl font-bold tracking-tighter sm:text-5xl">
                  Vores Team
                </h2>
                <p className="max-w-[900px] text-gray-500 md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                  Mød de dedikerede mennesker, der gør {config.CompanyName}{" "}
                  muligt.
                </p>
              </div>
            </div>
            <div className="mx-auto grid max-w-5xl grid-cols-1 gap-8 py-12 md:grid-cols-2 lg:grid-cols-3">
              <div className="flex flex-col items-center space-y-4">
                <img
                  src="/placeholder.svg?height=200&width=200"
                  alt="Mads Jensen"
                  className="rounded-full h-40 w-40 object-cover"
                />
                <div className="space-y-2 text-center">
                  <h3 className="text-xl font-bold">Mads Jensen</h3>
                  <p className="text-sm text-gray-500">Formand</p>
                  <p className="text-sm">
                    Studerende ved Københavns Universitet, Statskundskab. Har
                    været med siden foreningens start i 2018.
                  </p>
                </div>
              </div>
              <div className="flex flex-col items-center space-y-4">
                <img
                  src="/placeholder.svg?height=200&width=200"
                  alt="Sofie Nielsen"
                  className="rounded-full h-40 w-40 object-cover"
                />
                <div className="space-y-2 text-center">
                  <h3 className="text-xl font-bold">Sofie Nielsen</h3>
                  <p className="text-sm text-gray-500">Næstformand</p>
                  <p className="text-sm">
                    Studerende ved CBS, International Business. Har været med i
                    foreningen siden 2020.
                  </p>
                </div>
              </div>
              <div className="flex flex-col items-center space-y-4">
                <img
                  src="/placeholder.svg?height=200&width=200"
                  alt="Anders Petersen"
                  className="rounded-full h-40 w-40 object-cover"
                />
                <div className="space-y-2 text-center">
                  <h3 className="text-xl font-bold">Anders Petersen</h3>
                  <p className="text-sm text-gray-500">Kasserer</p>
                  <p className="text-sm">
                    Studerende ved DTU, Softwareteknologi. Har været med i
                    foreningen siden 2021.
                  </p>
                </div>
              </div>
              <div className="flex flex-col items-center space-y-4">
                <img
                  src="/placeholder.svg?height=200&width=200"
                  alt="Camilla Andersen"
                  className="rounded-full h-40 w-40 object-cover"
                />
                <div className="space-y-2 text-center">
                  <h3 className="text-xl font-bold">Camilla Andersen</h3>
                  <p className="text-sm text-gray-500">Rejsekoordinator</p>
                  <p className="text-sm">
                    Studerende ved Aarhus Universitet, Antropologi. Har været
                    med i foreningen siden 2019.
                  </p>
                </div>
              </div>
              <div className="flex flex-col items-center space-y-4">
                <img
                  src="/placeholder.svg?height=200&width=200"
                  alt="Mikkel Hansen"
                  className="rounded-full h-40 w-40 object-cover"
                />
                <div className="space-y-2 text-center">
                  <h3 className="text-xl font-bold">Mikkel Hansen</h3>
                  <p className="text-sm text-gray-500">PR-ansvarlig</p>
                  <p className="text-sm">
                    Studerende ved RUC, Kommunikation. Har været med i
                    foreningen siden 2022.
                  </p>
                </div>
              </div>
              <div className="flex flex-col items-center space-y-4">
                <img
                  src="/placeholder.svg?height=200&width=200"
                  alt="Line Christensen"
                  className="rounded-full h-40 w-40 object-cover"
                />
                <div className="space-y-2 text-center">
                  <h3 className="text-xl font-bold">Line Christensen</h3>
                  <p className="text-sm text-gray-500">Eventkoordinator</p>
                  <p className="text-sm">
                    Studerende ved Københavns Universitet, Sociologi. Har været
                    med i foreningen siden 2021.
                  </p>
                </div>
              </div>
            </div>
          </div>
        </section>
        <section className="w-full py-12 md:py-24 lg:py-32">
          <div className="container px-4 md:px-6 mx-auto">
            <div className="flex flex-col items-center justify-center space-y-4 text-center">
              <div className="space-y-2">
                <h2 className="text-3xl font-bold tracking-tighter sm:text-5xl">
                  Støt Vores Arbejde
                </h2>
                <p className="max-w-[900px] text-gray-500 md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                  Din donation hjælper os med at arrangere flere{" "}
                  {config.CompanyName} og gøre dem mere tilgængelige for alle
                  studerende.
                </p>
              </div>
                <DonateButton size={"lg"}>Donér Nu</DonateButton>
            </div>
          </div>
        </section>
      </main>
    </div>
  );
}
