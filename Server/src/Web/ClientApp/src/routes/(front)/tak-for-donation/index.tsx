import { Button } from "@/components/ui/button";
import { GlobeIcon, CheckCircle } from "lucide-react";
import { config } from "@/lib/config";
import { createFileRoute, Link } from "@tanstack/react-router";
import LinkButton from "@/components/shared/link-button";

export const Route = createFileRoute("/(front)/tak-for-donation/")({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <div className="flex flex-col min-h-screen">
      <main className="flex-1">
        <section className="w-full py-12 md:py-24 lg:py-32 bg-gradient-to-r from-blue-50 to-indigo-50">
          <div className="container px-4 md:px-6 mx-auto">
            <div className="flex flex-col items-center justify-center space-y-4 text-center">
              <div className="space-y-2">
                <CheckCircle className="mx-auto h-16 w-16 text-green-500" />
                <h1 className="text-3xl font-bold tracking-tighter sm:text-4xl md:text-5xl">
                  Tak for din donation!
                </h1>
                <p className="max-w-[600px] text-gray-500 md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                  Din støtte betyder meget for os og hjælper os med at fortsætte
                  vores arbejde med at arrangere
                  {config.CompanyName}.
                </p>
              </div>
              <div className="flex flex-col gap-2 min-[400px]:flex-row mt-6">
                <LinkButton to="/" className="flex-1">
                  Gå til forsiden
                </LinkButton>
                <LinkButton variant="outline" className="flex-1" to="/rejser">
                  <GlobeIcon className="mr-2 h-4 w-4" />
                  Se vores rejser
                </LinkButton>
              </div>
            </div>
          </div>
        </section>
        <section className="w-full py-12 md:py-24 lg:py-32">
          <div className="container px-4 md:px-6 mx-auto">
            <div className="grid gap-6 lg:grid-cols-2 lg:gap-12 items-center">
              <div className="space-y-4">
                <h2 className="text-3xl font-bold tracking-tighter">
                  Hvad går din donation til?
                </h2>
                <p className="text-gray-500 md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                  Din donation hjælper os med at:
                </p>
                <ul className="space-y-2 text-gray-500 md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                  <li className="flex items-center">
                    <CheckCircle className="mr-2 h-5 w-5 text-green-500" />
                    Arrangere flere {config.CompanyName} til spændende
                    destinationer
                  </li>
                  <li className="flex items-center">
                    <CheckCircle className="mr-2 h-5 w-5 text-green-500" />
                    Tilbyde rejsestipendier til studerende med begrænsede midler
                  </li>
                  <li className="flex items-center">
                    <CheckCircle className="mr-2 h-5 w-5 text-green-500" />
                    Arrangere kulturelle og akademiske aktiviteter under
                    rejserne
                  </li>
                  <li className="flex items-center">
                    <CheckCircle className="mr-2 h-5 w-5 text-green-500" />
                    Skabe netværksmuligheder med internationale studerende
                  </li>
                </ul>
              </div>
              <img
                src="/placeholder.svg?height=550&width=800"
                alt="Studerende på rejse"
                className="mx-auto aspect-video overflow-hidden rounded-xl object-cover object-center sm:w-full"
              />
            </div>
          </div>
        </section>
      </main>
    </div>
  );
}
