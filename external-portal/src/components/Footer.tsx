import { Box, Container, Link, Stack, Typography } from "@mui/material";

export default function Footer(){
  return (
    <Box sx={{ bgcolor:"#111", color:"#ccc", py:4, mt:6 }}>
      <Container>
        <Stack direction={{xs:"column", sm:"row"}} justifyContent="space-between" spacing={2}>
          <Typography>© {new Date().getFullYear()} Carpenters Fiji</Typography>
          <Stack direction="row" spacing={3}>
            <Link href="#" color="inherit" underline="hover">Privacy</Link>
            <Link href="#" color="inherit" underline="hover">Careers</Link>
            <Link href="#" color="inherit" underline="hover">Contact</Link>
          </Stack>
        </Stack>
      </Container>
    </Box>
  );
}
