import Grid from "@mui/material/GridLegacy";
import Container from "@mui/material/Container";
import Card from "@mui/material/Card";
import CardActionArea from "@mui/material/CardActionArea";
import CardContent from "@mui/material/CardContent";
import Typography from "@mui/material/Typography";



const units = [
  {k:"retail", t:"Retail & Supermarkets"},
  {k:"automotive", t:"Automotive"},
  {k:"property", t:"Property"},
  {k:"finance", t:"Finance & Accounting"},
  {k:"it", t:"IT & Digital"},
  {k:"hr", t:"Human Resources"},
];

export default function UnitsCarousel(){
  return (
    <Container sx={{ py: 6 }}>
      <Typography variant="h5" gutterBottom>Career Areas</Typography>
      <Grid container spacing={2}>
        {units.map(u=>(
          <Grid item xs={12} sm={6} md={4} key={u.k}>
            <Card variant="outlined">
              <CardActionArea href={`/jobs?unit=${u.k}`}>
                <CardContent>
                  <Typography variant="subtitle1">{u.t}</Typography>
                  <Typography variant="body2" color="text.secondary">Explore roles in {u.t}.</Typography>
                </CardContent>
              </CardActionArea>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Container>
  );
}
