// src/components/Header.tsx
import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import Container from "@mui/material/Container";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import Stack from "@mui/material/Stack";

export default function Header() {
  return (
    <AppBar
      position="sticky"
      elevation={0}
      sx={{
        bgcolor: "#FFF200",    // Carpenters yellow
        color: "#111",         // dark text
        borderBottom: "1px solid rgba(0,0,0,0.08)",
      }}
    >
      <Container maxWidth="lg">
        <Toolbar disableGutters sx={{ minHeight: 64 }}>
          {/* LEFT: logo + brand */}
          <Stack direction="row" alignItems="center" spacing={1}>
            <Box
              component="img"
              src="logo-carpenters-motors.PNG"
              sx={{ width: 40, height: 40, objectFit: "contain" }}
            />
            <Typography
              variant="h6"
              sx={{ fontWeight: 800, letterSpacing: 0.5 }}
            >
              CARPENTERS FIJI PTE LIMITED
            </Typography>
          </Stack>
<Stack direction="row" spacing={3} sx={{ ml: "auto" }}>
  <Button href="/" variant="text" sx={{ color: "#111", fontWeight: 700 }}>
    Home
  </Button>
  <Button href="/jobs" variant="text" sx={{ color: "#111", fontWeight: 700 }}>
    Careers
  </Button>
 <Button
  color="inherit"
  sx={{ fontWeight: 700 }} 
  onClick={() => {
    document.getElementById("about-us")?.scrollIntoView({
      behavior: "smooth",
      block: "start",   // aligns top of section with viewport
      inline: "nearest"
    });
  }}
>
ABOUT US
</Button>

  <Button href="#contact" variant="text" sx={{ color: "#111", fontWeight: 700 }}>
    Contact
  </Button>
</Stack>
        </Toolbar>
      </Container>
    </AppBar>
  );
}
