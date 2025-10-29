import { useEffect, useMemo, useRef } from "react";
import {
  useMotionValue,
  useTransform,
  animate,
  motion,
  useInView,
} from "framer-motion";

interface AnimatedTextProps {
  children: string | string[];
  wordsPerMinute?: number;
  className?: string;
}

export default function AnimatedText({
  children: text,
  wordsPerMinute = 200,
  className,
}: AnimatedTextProps) {
  const ref = useRef<HTMLSpanElement | null>(null);
  const isInView = useInView(ref);

  const string = useMemo(() => {
    return Array.isArray(text) ? text.join(" ") : text;
  }, [text]);

  const duration = useMemo(() => {
    const wordCount = string.split(" ").length;
    return (wordCount / wordsPerMinute) * 60;
  }, [string, wordsPerMinute]);

  const count = useMotionValue(0);
  const rounded = useTransform(count, (latest) => Math.round(latest));
  const displayText = useTransform(rounded, (latest) =>
    latest ? string.slice(0, latest) : "\u00A0"
  );

  useEffect(() => {
    if (!isInView) return;

    const controls = animate(count, string.length, {
      type: "tween",
      duration: duration,
      ease: "linear",
    });

    return controls.stop;
  }, [string, duration, isInView]);

  return (
    <div>
      <motion.span ref={ref} className={className}>
        {displayText}
      </motion.span>
    </div>
  );
}
