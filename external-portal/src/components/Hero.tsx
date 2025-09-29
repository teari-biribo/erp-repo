import { Box, Button, Container, Typography } from "@mui/material";
import SearchBar from "./SearchBar";
import { useEffect, useMemo, useRef, useState } from "react";

const IMAGE_PATHS = [
  "public/hero/1.jpg",
  "public/hero/2.jpg",
  "public/hero/3.jpg",
  "public/hero/4.jpg",
  "public/hero/5.jpeg",
  "public/hero/6.jpg",
  "public/hero/7.jpeg",
  "public/hero/8.jpeg",
  "public/hero/9.jpg",
  "public/hero/10.jpeg",
];

const SLIDE_MS = 3000;     // 6 seconds per image
const FADE_MS  = 900;      // fade duration

export default function Hero() {
  // current slide index
  const [idx, setIdx] = useState(0);

  // pre-load images once (speeds up the first cycle)
  useEffect(() => {
    IMAGE_PATHS.forEach((src) => {
      const img = new Image();
      img.src = src;
    });
  }, []);

  // auto-advance
  useEffect(() => {
    const t = setInterval(() => {
      setIdx((i) => (i + 1) % IMAGE_PATHS.length);
    }, SLIDE_MS);
    return () => clearInterval(t);
  }, []);

  const wrapperRef = useRef<HTMLDivElement>(null);

// pause on hover
useEffect(() => {
  const el = wrapperRef.current;
  if (!el) return;
  let paused = false;
  const onEnter = () => { paused = true; };
  const onLeave = () => { paused = false; };
  el.addEventListener("mouseenter", onEnter);
  el.addEventListener("mouseleave", onLeave);

  const id = setInterval(() => {
    if (!paused) setIdx((i) => (i + 1) % IMAGE_PATHS.length);
  }, SLIDE_MS);

  return () => {
    el.removeEventListener("mouseenter", onEnter);
    el.removeEventListener("mouseleave", onLeave);
    clearInterval(id);
  };
}, []);

  // two-layer cross-fade: show current + previous with opacity transition
  const prevIdx = useRef(0);
  useEffect(() => { prevIdx.current = idx; }, [idx]);

  const layers = useMemo(() => {
    const curr = IMAGE_PATHS[idx];
    const prev = IMAGE_PATHS[prevIdx.current];
    // if first render, use same image for both so there is no flash
    return [prev ?? curr, curr];
  }, [idx]);

  return (
    <Box sx={{ position: "relative", overflow: "hidden" }}>
      {/* Background layers */}
      {layers.map((src, i) => (
        <Box
          key={i}
          sx={{
            position: "absolute",
            inset: 0,
            backgroundImage: `linear-gradient(90deg, rgba(0,0,0,0.55), rgba(0,0,0,0.25)), url('${src}')`,
            backgroundSize: "cover",
            backgroundPosition: "center",
            transition: `opacity ${FADE_MS}ms ease`,
            opacity: i === 1 ? 1 : 0, // top layer = current slide
          }}
        />
      ))}

      {/* Content sits above the background */}
      <Container sx={{ py: { xs: 8, md: 12 }, position: "relative", zIndex: 1 }}>
        <Typography variant="h2" fontWeight={800} color="white" sx={{ lineHeight: 1.1 }}>
          Your Future, Built Here
        </Typography>
        <Typography variant="h6" sx={{ mt: 1, maxWidth: 720, color: "rgba(255,255,255,0.9)" }}>
          Join Carpenters Fiji and help deliver great experiences across our businesses.
        </Typography>

        <Box sx={{ mt: 3, maxWidth: 900 }}>
          <SearchBar />
        </Box>

        <Button
          href="#roles"
          variant="outlined"
          sx={{
            mt: 2,
            color: "white",
            borderColor: "white",
            ":hover": { borderColor: "white", backgroundColor: "rgba(255,255,255,0.12)" },
          }}
        >
          Explore roles
        </Button>
      </Container>
    </Box>
  );
}
