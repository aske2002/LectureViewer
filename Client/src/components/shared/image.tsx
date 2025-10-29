import { ResourceResponse } from "@/app/web-api-client";
import React, { useMemo } from "react";

type ImageProps = {
  src: ResourceResponse | string | undefined;
} & Omit<React.ImgHTMLAttributes<HTMLImageElement>, "src">;

export default function ResourceImage(props: ImageProps) {
  const { src, ...rest } = props;
  const imageSrc = useMemo(() => {
    if (src instanceof ResourceResponse) {
      return src.url;
    }
    return src;
  }, [src]);

  return <img src={imageSrc} {...rest} />;
}
