import { Box, Button, Stack, TextField } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { useState } from "react";

export default function SearchBar() {
  const [q, setQ] = useState("");
  const [loc, setLoc] = useState("");
  const navigate = useNavigate();

  const submit = (e: React.FormEvent) => {
    e.preventDefault();
    const params = new URLSearchParams();
    if (q) params.set("q", q);
    if (loc) params.set("loc", loc);
    navigate(`/jobs?${params.toString()}`);
  };

  return (
    <Box component="form" onSubmit={submit} sx={{ width: "100%" }}>
      <Stack
        direction={{ xs: "column", md: "row" }}
        spacing={2}
        alignItems="stretch"
      >
        {/* Search for jobs (WHITE) */}
        <TextField
          fullWidth
          placeholder="Search for jobs"
          value={q}
          onChange={(e) => setQ(e.target.value)}
          variant="outlined"
          // make it look like a clean white box
          sx={{
            "& .MuiOutlinedInput-root": {
              backgroundColor: "#fff",
              borderRadius: 1.5, // 12px
              height: 56,
              // remove the grey outline
              "& fieldset": { borderColor: "transparent" },
              "&:hover fieldset": { borderColor: "transparent" },
              "&.Mui-focused fieldset": { borderColor: "transparent" },
              boxShadow: "0 2px 6px rgba(0,0,0,0.15)",
            },
            "& input::placeholder": { color: "rgba(0,0,0,0.6)" },
          }}
          inputProps={{ "aria-label": "search for jobs" }}
        />

        {/* Enter location (WHITE) */}
        <TextField
          fullWidth
          placeholder="Enter location"
          value={loc}
          onChange={(e) => setLoc(e.target.value)}
          variant="outlined"
          sx={{
            "& .MuiOutlinedInput-root": {
              backgroundColor: "#fff",
              borderRadius: 1.5,
              height: 56,
              "& fieldset": { borderColor: "transparent" },
              "&:hover fieldset": { borderColor: "transparent" },
              "&.Mui-focused fieldset": { borderColor: "transparent" },
              boxShadow: "0 2px 6px rgba(0,0,0,0.15)",
            },
            "& input::placeholder": { color: "rgba(0,0,0,0.6)" },
          }}
          inputProps={{ "aria-label": "enter location" }}
        />

        {/* Find Jobs (RED) */}
<Button
  type="submit"
  variant="contained"
  size="large"
  sx={{
    bgcolor: "#d40511", // red
    color: "#fff",
    borderRadius: 2,
    height: 52,              // same height as TextFields
    px: 3,                   // button width
    fontWeight: 500,
    fontSize: "14px",
    textTransform: "none",   // keep "Find Jobs" as typed
    whiteSpace: "nowrap",    // <-- prevents breaking into 2 lines
    boxShadow: "0 2px 6px rgba(0,0,0,0.15)",
    "&:hover": { bgcolor: "#b3040f" },
  }}
>
 Find Jobs
</Button>
      </Stack>
    </Box>
  );
}
