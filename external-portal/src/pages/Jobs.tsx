// src/pages/Jobs.tsx
import {
  Box,
  Container,
  Typography,
  Card,
  CardContent,
  Button,
  CardActionArea,
} from "@mui/material";
import WorkIcon from "@mui/icons-material/Work";
import LocationOnIcon from "@mui/icons-material/LocationOn";
import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import Grid from "@mui/material/GridLegacy";

type Job = {
  id: number;
  title: string;
  company: string;
  location: string;
  logo: string;
  type: string;
  date: string;
};

export default function Jobs() {
  const [jobs, setJobs] = useState<Job[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    fetch("/jobs.json")
      .then((res) => res.json())
      .then((data) => setJobs(data))
      .catch((err) => console.error("Failed to load jobs:", err));
  }, []);

  return (
    <Box sx={{ flexGrow: 1, bgcolor: "#fff", py: 6 }}>
      <Container maxWidth="lg">
        <Typography
          variant="h5"
          fontWeight={700}
          align="center"
          gutterBottom
          sx={{ mb: 4 }}
        >
          LATEST JOBS
        </Typography>

        <Grid container spacing={2}>
          {jobs.map((job) => (
            <Grid item xs={12} key={job.id}>
              <Card sx={{ borderRadius: 2, boxShadow: "0 2px 6px rgba(0,0,0,0.1)" }}>
                <CardActionArea onClick={() => navigate(`/jobs/${job.id}`)}>
                  <Box sx={{ display: "flex", alignItems: "center", p: 2 }}>
                    {/* Logo + Company under it */}
                    <Box sx={{ flexShrink: 0, mr: 2, textAlign: "center" }}>
                      <img
                        src={job.logo || "/default-logo.png"}
                        alt={job.company}
                        style={{
                          width: 60,
                          height: 60,
                          objectFit: "contain",
                          display: "block",
                          margin: "0 auto",
                        }}
                      />
                      <Typography
                        variant="caption"
                        sx={{ display: "block", mt: 1, fontWeight: "bold" }}
                      >
                        {job.company}
                      </Typography>
                    </Box>

                    {/* Center content */}
                    <CardContent sx={{ flex: 1, py: 0 }}>
                      <Typography
                        variant="h6"
                        sx={{ fontWeight: 700, color: "#c00", mb: 0.5 }}
                      >
                        {job.title}
                      </Typography>

                      {/* Company */}
                      <Box sx={{ display: "flex", alignItems: "center", mb: 0.5 }}>
                        <WorkIcon
                          sx={{ fontSize: 18, color: "text.secondary", mr: 1 }}
                        />
                        <Typography variant="body2" color="text.secondary">
                          {job.company}
                        </Typography>
                      </Box>

                      {/* Location */}
                      <Box sx={{ display: "flex", alignItems: "center" }}>
                        <LocationOnIcon
                          sx={{ fontSize: 18, color: "text.secondary", mr: 1 }}
                        />
                        <Typography variant="body2" color="text.secondary">
                          {job.location}
                        </Typography>
                      </Box>
                    </CardContent>

                    {/* Right side */}
                    <Box sx={{ textAlign: "right", minWidth: 120 }}>
                      <Typography
                        variant="body2"
                        sx={{ fontWeight: 600, mb: 1, color: "text.primary" }}
                      >
                        {job.date}
                      </Typography>
                      <Button
                        variant="outlined"
                        size="small"
                        sx={{
                          borderColor: "#c00",
                          color: "#c00",
                          fontWeight: 600,
                          borderRadius: "20px",
                          textTransform: "uppercase",
                          pointerEvents: "none",
                        }}
                      >
                        {job.type}
                      </Button>
                    </Box>
                  </Box>
                </CardActionArea>
              </Card>
            </Grid>
          ))}
        </Grid>

        {jobs.length === 0 && (
          <Typography align="center" sx={{ mt: 4 }}>
            No jobs available right now.
          </Typography>
        )}
      </Container>
    </Box>
  );
}
