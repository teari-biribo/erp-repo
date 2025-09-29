import { createBrowserRouter } from "react-router-dom";
import SiteLayout from "../components/SiteLayout";
import Home from "../pages/Home";
import Jobs from "../pages/Jobs";

export const router = createBrowserRouter([
  { path: "/", element: <SiteLayout><Home /></SiteLayout> },
  { path: "/jobs", element: <SiteLayout><Jobs /></SiteLayout> },
  { path: "*", element: <div>Not Found</div> },
]);
