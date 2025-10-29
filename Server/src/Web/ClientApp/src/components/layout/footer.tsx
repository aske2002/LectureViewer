import { config } from "@/lib/config";
import { Link } from "@tanstack/react-router";
import Logo from "../logo/logo";

export default function Footer() {
  return (
    <footer className="border-t bg-gray-100">
      <div className="container flex flex-col gap-6 py-8 px-4 md:px-6 md:flex-row md:justify-between">
        <div className="flex flex-col gap-2">
          <Link to="/" className="flex items-center gap-2 font-bold text-xl">
            <Logo className="h-12" />
          </Link>
          <p className="text-sm text-gray-500">
            © 2025 {config.CompanyName}. Alle rettigheder forbeholdes.
          </p>
        </div>
        <div className="grid grid-cols-2 gap-10 sm:grid-cols-3">
          <div className="space-y-3">
            <h3 className="text-sm font-medium">Navigation</h3>
            <ul className="space-y-2 text-sm">
              <li>
                <Link to="/" className="hover:underline underline-offset-4">
                  Hjem
                </Link>
              </li>
              <li>
                <Link
                  to="/om-os"
                  className="hover:underline underline-offset-4"
                >
                  Om Os
                </Link>
              </li>
              <li>
                <Link
                  to="/rejser"
                  className="hover:underline underline-offset-4"
                >
                  Rejser
                </Link>
              </li>
              <li>
                <Link
                  to="/doner"
                  className="hover:underline underline-offset-4"
                >
                  Donér
                </Link>
              </li>
              <li>
                <Link
                  to="/kontakt"
                  className="hover:underline underline-offset-4"
                >
                  Kontakt
                </Link>
              </li>
            </ul>
          </div>
          <div className="space-y-3">
            <h3 className="text-sm font-medium">Kontakt</h3>
            <ul className="space-y-2 text-sm">
              <li>
                <a
                  href="mailto:info@{config.CompanyName}.dk"
                  className="hover:underline underline-offset-4"
                >
                  info@{config.CompanyName}.dk
                </a>
              </li>
              <li>
                <span>Finlandsgade 22, 8200 Aarhus N</span>
              </li>
              <li>
                <span>
                  CVR:{" "}
                  <a href="https://datacvr.virk.dk/enhed/virksomhed/45471160?fritekst=45471160&sideIndex=0&size=10">
                    45471160
                  </a>
                </span>
              </li>
            </ul>
          </div>
          <div className="space-y-3">
            <h3 className="text-sm font-medium">Sociale Medier</h3>
            <ul className="space-y-2 text-sm">
              <li>
                <a href="#" className="hover:underline underline-offset-4">
                  Facebook
                </a>
              </li>
              <li>
                <a href="#" className="hover:underline underline-offset-4">
                  Instagram
                </a>
              </li>
              <li>
                <a href="#" className="hover:underline underline-offset-4">
                  LinkedIn
                </a>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </footer>
  );
}
