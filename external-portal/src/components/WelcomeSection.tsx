import Container from "@mui/material/Container";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";

export default function WelcomeSection() {
  return (
    <Box sx={{ bgcolor: "#fff" }}>
      <Container maxWidth="lg" sx={{ py: { xs: 6, md: 8 }, textAlign: "center" }}>
        <Typography
          variant="h5"
          fontWeight={800}
          sx={{ letterSpacing: 1, mb: 2 }}
        >
          WELCOME TO CARPENTERS GROUP FIJI
        </Typography>

        <Typography
          variant="body1"
          sx={{ lineHeight: 1.8, maxWidth: 1100, mx: "auto" }}
        >
          Carpenters Fiji Pte Limited, a subsidiary of MBf Holdings, represents
          almost all sectors of MBf in Fiji with wholesale, retail, hardware,
          automotive, information technology, finance, water bottling, shipping
          and industrial and marine engineering industries.
        </Typography>
      </Container>
    </Box>
  );
}
