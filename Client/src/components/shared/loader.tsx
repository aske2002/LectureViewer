import React from "react";

interface LoaderProps {
  descriptionText?: string;
}

const Loader: React.FC<LoaderProps> = ({ descriptionText }) => {
  return (
    <div className="relative flex flex-col items-center justify-center gap-6">
      <div className="spinner relative w-11 h-11 animate-spinner-y0fdc1 transform-3d">
        {[...Array(6)].map((_, i) => (
          <div
            key={i}
            className="absolute w-full h-full border-2 border-primary bg-primary/20"
          />
        ))}
      </div>
      {descriptionText && (
        <div className="flex text-base font-medium tracking-wide text-primary">
          {descriptionText.split("").map((char, i) => (
            <span
              key={i}
              className={char === " " ? "w-1" : "inline-block animate-textWave"}
              style={char === " " ? {} : { animationDelay: `${i * 0.05}s` }}
            >
              {char === " " ? "\u00A0" : char}
            </span>
          ))}
        </div>
      )}
    </div>
  );
};

const FullScreenLoader: React.FC<LoaderProps> = (props) => {
  return (
    <div className="fixed inset-0 flex items-center justify-center bg-background z-50">
      <Loader {...props} />
    </div>
  );
};

export { Loader, FullScreenLoader };
