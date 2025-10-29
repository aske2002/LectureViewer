import { createFileRoute } from "@tanstack/react-router";
import { config } from "@/lib/config";
import Trip from "@/components/layout/trip";
import DonateButton from "@/components/shared/donate-button";
import LinkButton from "@/components/shared/link-button";
import { GlobeIcon } from "lucide-react";
import AnimatedText from "@/components/shared/animated-text";

export const Route = createFileRoute("/(front)/")({
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
                  Udforsk Verden Med {config.CompanyName}
                </h1>
                <p className="max-w-[600px] text-gray-500 md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                  Vi er en studenterforening dedikeret til, at arrangere
                  studieture til udlandet for sundhedsteknologi-studerende.
                  Vores mål er at give studerende mulighed for at opleve
                  forskellige kulturer og uddannelsessystemer.
                </p>
                <div className="flex flex-col gap-2 min-[400px]:flex-row">
                  <LinkButton className="flex-1" to="/rejser">
                    <GlobeIcon className="mr-2 h-4 w-4" />
                    Se vores rejser
                  </LinkButton>
                  <DonateButton variant={"outline"}>
                    Støt vores arbejde
                  </DonateButton>
                </div>
              </div>
              <img
                src="/placeholder.svg?height=550&width=800"
                alt="Studerende på rejse"
                className="mx-auto aspect-video overflow-hidden rounded-xl object-cover object-center sm:w-full"
              />
            </div>
          </div>
        </section>
        <section className="w-full py-12 md:py-24 lg:py-32">
          <div className="container px-4 md:px-6 mx-auto">
            <div className="flex flex-col items-center justify-center space-y-4 text-center">
              <div className="space-y-2">
                <div className="inline-block rounded-lg bg-gray-100 px-3 py-1 text-sm">
                  Kommende Rejser
                </div>
                <AnimatedText className="text-3xl font-bold tracking-tighter sm:text-5xl">
                  Oplev verden med os.
                </AnimatedText>
                <p className="max-w-[900px] text-gray-500 md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                  Nedenfor ses en liste over {config.CompanyName}'s kommende
                  rejser. Klik på en rejse for at læse mere.
                </p>
              </div>
            </div>
            <div className="mx-auto grid max-w-5xl grid-cols-1 gap-6 py-12 md:grid-cols-2 lg:grid-cols-3">
              {config.Trips.map((trip) => (
                <Trip key={trip.id} trip={trip} />
              ))}
            </div>
          </div>
        </section>
        <section className="w-full py-12 md:py-24 lg:py-32 bg-gray-100">
          <div className="container px-4 md:px-6 mx-auto">
            <div className="grid gap-6 lg:grid-cols-2 lg:gap-12 items-center">
              <div className="space-y-4">
                <h2 className="text-3xl font-bold tracking-tighter sm:text-4xl md:text-5xl">
                  Om {config.CompanyName}
                </h2>
                <p className="max-w-[600px] text-gray-500 md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                  {config.CompanyName} blev grundlagt i 2025 af en gruppe
                  studerende fra Aarhus Universitet med en passion for rejser og
                  kulturel udveksling. Formålet med foreningen er at arrangere
                  relevante studieture for studerende på sundhedsteknologi.
                </p>
                <p className="max-w-[600px] text-gray-500 md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                  Vores mål er at give studerende mulighed for at opleve
                  forskellige uddannelsessystemer, kulturer og traditioner,
                  samtidig med at de knytter værdifulde kontakter med
                  internationale studerende.
                </p>
                <div className="flex flex-col gap-2 min-[400px]:flex-row">
                  <LinkButton to="/om-os">Læs Mere Om Os</LinkButton>
                </div>
              </div>
              <div className="grid grid-cols-2 gap-4">
                <img
                  src="/homepage/1.jpeg?height=300&width=400"
                  alt="Studerende på rejse"
                  className="rounded-lg object-cover aspect-square"
                />
                <img
                  src="/homepage/2.jpeg?height=300&width=400"
                  alt="Studerende på rejse"
                  className="rounded-lg object-cover aspect-square"
                />
                <img
                  src="/homepage/3.jpeg?height=300&width=400"
                  alt="Studerende på rejse"
                  className="rounded-lg object-cover aspect-square"
                />
                <img
                  src="/homepage/4.jpeg?height=300&width=400"
                  alt="Studerende på rejse"
                  className="rounded-lg object-cover aspect-square"
                />
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
                  Din donation hjælper os med at arrangere studieture for de
                  studerende på sundhedsteknologi, såsom transport, logi og
                  studierelevante besøg.
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
