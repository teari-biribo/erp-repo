// src/components/ValuesBand.tsx
import Grid from "@mui/material/GridLegacy";          // classic Grid (has 'item')
import Container from "@mui/material/Container";
import Paper from "@mui/material/Paper";
import Typography from "@mui/material/Typography";

const values = [
  {
    title: "Mission",
    className: "mission-bg",
    text: "To provide quality products and services that improve the lives of our customers, create sustainable opportunities for our employees, and contribute to the economic growth of Fiji and the Pacific.",
  },
  {
    title: "Vision",
    className: "vision-bg",
    text: "To be the most trusted and innovative business group in Fiji and the Pacific, recognized for excellence, customer care, and community leadership.",
  },
  {
    title: "Core Values",
    className: "values-bg",
    text: "Driven by Integrity, Focused on Customers, Powered by Innovation, United through Teamwork, and Committed to Sustainability.",
  },
];

export default function ValuesBand() {
  return (
    <Container maxWidth={false} disableGutters sx={{ py: { xs: 4, md: 6 } }}>
      <Grid container spacing={0} alignItems="stretch">
        {values.map((v, i) => (
          <Grid key={i} item xs={12} md={4}>
            <Paper
              elevation={0}
              sx={{
                height: "100%",
                display: "flex",
                flexDirection: "column",
                p: { xs: 3, md: 4 },
                border: "none",
                boxShadow: "none",
              }}
            >
              <Typography variant="h5" fontWeight={700} gutterBottom>
                {v.title}
              </Typography>
              <Typography color="text.secondary">{v.text}</Typography>
            </Paper>
          </Grid>
        ))}
      </Grid>
    </Container>
  );
}
