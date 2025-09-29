import { Chip, Container, Stack, Typography } from "@mui/material";

const locations = ["Suva", "Lautoka", "Nadi", "Labasa", "Sigatoka"];

export default function LocationsStrip(){
  return (
    <Container sx={{ py: 6 }}>
      <Typography variant="h5" gutterBottom>Our Locations</Typography>
      <Stack direction="row" spacing={1} flexWrap="wrap">
        {locations.map(l => (
          <Chip key={l} label={`Jobs in ${l}`} component="a" href={`/jobs?loc=${encodeURIComponent(l)}`} clickable />
        ))}
      </Stack>
    </Container>
  );
}
