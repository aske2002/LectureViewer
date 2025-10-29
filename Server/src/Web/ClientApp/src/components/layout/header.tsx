import { Link } from "@tanstack/react-router";
import Logo from "../logo/logo";
import DonateButton from "../shared/donate-button";

export default function Header() {
  return (
    <header className="sticky top-0 bg-white border-b z-30">
      <div className="container flex items-center justify-between h-16 px-4 mx-auto md:px-6">
        <Link to="/" className="flex items-center gap-2 h-full py-4">
          <Logo
            style={{
              height: "100%",
            }}
          />
        </Link>
        <nav className="hidden md:flex items-center gap-6">
          <Link
            to="/"
            className="text-sm font-medium hover:underline underline-offset-4"
          >
            Hjem
          </Link>
          <Link
            to="/om-os"
            className="text-sm font-medium hover:underline underline-offset-4"
          >
            Om Os
          </Link>
          <Link
            to="/rejser"
            className="text-sm font-medium hover:underline underline-offset-4"
          >
            Rejser
          </Link>
          <Link
            to="/doner"
            className="text-sm font-medium hover:underline underline-offset-4"
          >
            Donér
          </Link>
          <Link
            to="/kontakt"
            className="text-sm font-medium hover:underline underline-offset-4"
          >
            Kontakt
          </Link>
        </nav>
        <div className="flex items-center gap-4">
          <DonateButton>Støt os</DonateButton>
          <button className="md:hidden">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="24"
              height="24"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="2"
              strokeLinecap="round"
              strokeLinejoin="round"
              className="h-6 w-6"
            >
              <line x1="4" x2="20" y1="12" y2="12" />
              <line x1="4" x2="20" y1="6" y2="6" />
              <line x1="4" x2="20" y1="18" y2="18" />
            </svg>
          </button>
        </div>
      </div>
    </header>
  );
}
