import { useState } from "react";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { config } from "@/lib/config";
import { createFileRoute } from "@tanstack/react-router";
import DonateValue from "@/components/shared/donate-value";

export const Route = createFileRoute("/(front)/doner/")({
  component: RouteComponent,
});

const defaultValues = [50, 100, 200, 500, 1000, 2000];

function RouteComponent() {
  const [amount, setAmount] = useState<number>(100);
  const [customAmount, setCustomAmount] = useState<string>("");
  const [isLoading, setIsLoading] = useState<boolean>(false);

  return (
    <div className="flex flex-col min-h-screen">
      
      <main className="flex-1">
        <section className="w-full py-12 md:py-24 lg:py-32 bg-gradient-to-r from-blue-50 to-indigo-50">
          <div className="container px-4 md:px-6 mx-auto">
            <div className="grid gap-6 lg:grid-cols-2 lg:gap-12 items-center">
              <div className="space-y-4">
                <h1 className="text-3xl font-bold tracking-tighter sm:text-4xl md:text-5xl">
                  Støt Vores Arbejde
                </h1>
                <p className="max-w-[600px] text-gray-500 md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                  Din donation hjælper os med at arrangere flere{" "}
                  {config.CompanyName} og gøre dem mere tilgængelige for alle
                  studerende.
                </p>
              </div>
              <img
                src="/placeholder.svg?height=550&width=800"
                alt="Studerende på rejse"
                className="mx-auto aspect-video overflow-hidden rounded-xl object-cover object-center sm:w-full"
              />
            </div>
          </div>
        </section>
        <section className="w-full py-12 md:py-24 lg:py-32" id="now">
          <div className="container px-4 md:px-6 mx-auto">
            <div className="max-w-md space-y-6 mx-auto">
              <Card>
                <CardHeader>
                  <CardTitle className="text-2xl">
                    Donér til {config.CompanyName}
                  </CardTitle>
                  <CardDescription>
                    Vælg et beløb eller indtast dit eget for at støtte vores
                    arbejde.
                  </CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                  <Tabs defaultValue="predefined" className="w-full">
                    <TabsList className="grid w-full grid-cols-2">
                      <TabsTrigger value="predefined">Vælg beløb</TabsTrigger>
                      <TabsTrigger value="custom">Eget beløb</TabsTrigger>
                    </TabsList>
                    <TabsContent value="predefined" className="space-y-4">
                      <RadioGroup
                        value={amount.toString()}
                        className="grid grid-cols-3 gap-4"
                        onValueChange={(value) => {
                          setAmount(Number.parseInt(value));
                        }}
                      >
                        {defaultValues.map((value) => (
                          <DonateValue key={value} value={value} />
                        ))}
                      </RadioGroup>
                    </TabsContent>
                    <TabsContent value="custom" className="space-y-4">
                      <div className="space-y-2">
                        <Label htmlFor="custom-amount">
                          Indtast beløb (DKK)
                        </Label>
                        <Input
                          id="custom-amount"
                          type="number"
                          placeholder="Indtast beløb"
                          value={customAmount}
                          onChange={(e) => setCustomAmount(e.target.value)}
                        />
                      </div>
                    </TabsContent>
                  </Tabs>
                </CardContent>
                <CardFooter>
                  <Button
                    className="w-full"
                    disabled={isLoading}
                  >
                    {isLoading ? "Behandler..." : "Donér nu"}
                  </Button>
                </CardFooter>
              </Card>
              <div className="space-y-4 text-center">
                <h2 className="text-xl font-bold">
                  Hvad går din donation til?
                </h2>
                <div className="grid gap-4 md:grid-cols-3">
                  <div className="rounded-lg border bg-card p-4 text-card-foreground shadow-sm">
                    <h3 className="font-medium">Rejsestøtte</h3>
                    <p className="text-sm text-gray-500">
                      Hjælper studerende med begrænsede midler til at deltage i
                      vores rejser.
                    </p>
                  </div>
                  <div className="rounded-lg border bg-card p-4 text-card-foreground shadow-sm">
                    <h3 className="font-medium">Arrangementer</h3>
                    <p className="text-sm text-gray-500">
                      Finansierer kulturelle og akademiske arrangementer under
                      vores rejser.
                    </p>
                  </div>
                  <div className="rounded-lg border bg-card p-4 text-card-foreground shadow-sm">
                    <h3 className="font-medium">Administration</h3>
                    <p className="text-sm text-gray-500">
                      Dækker administrative omkostninger for at holde foreningen
                      kørende.
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>
      </main>
    </div>
  );
}
