import Container from "@mui/material/Container";
import Typography from "@mui/material/Typography";

export default function AboutUs() {
  return (
    <Container
      id="about-us" // anchor for smooth scroll
      maxWidth={false}
      disableGutters
      sx={{ py: { xs: 6, md: 8 }, textAlign: "left",scrollMarginTop: "100px" }}
    >
      {/* Heading */}
      <Typography
        variant="h5"
        fontWeight={700}
        gutterBottom
        sx={{ mb: 2, ml: { xs: 2, md: 3 } }}  // align with Mission
      >
        About Us
      </Typography>

      {/* Paragraph */}
      <Typography
        color="text.secondary"
        sx={{
          mx: { xs: 2, md: 3 }, // same margin as Mission
          whiteSpace: "nowrap",  // keep all in one line
          overflow: "hidden",
          textOverflow: "ellipsis", // prevent breaking
        }}
      >
        Carpenters Fiji Pte Limited is one of the largest and most diversified
        companies in Fiji, committed to excellence, innovation, and sustainable
        growth across multiple sectors.
      </Typography>
    </Container>
  );
}

