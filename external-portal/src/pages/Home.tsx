import Hero from "../components/Hero";
import ValuesBand from "../components/ValuesBand";
import WelcomeSection from "../components/WelcomeSection";
import Stories from "../components/Stories";
import UnitsCarousel from "../components/UnitsCarousel";
import LocationsStrip from "../components/LocationsStrip";


export default function Home(){
  return (
    <>
      <Hero />
       {/* NEW: Welcome section */}
      <WelcomeSection />
      <div id="about">
  <ValuesBand />
</div>

      <Stories />
      <UnitsCarousel />
      <LocationsStrip />
      <div id="contact">
</div>
    </>
  );
}
