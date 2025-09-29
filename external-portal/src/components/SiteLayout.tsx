// src/components/SiteLayout.tsx
import Header from "./Header";
import Footer from "./Footer";

export default function SiteLayout({ children }: { children: React.ReactNode }) {
  return (
    <div className="site-wrapper">
      <Header />
      <main className="site-main">{children}</main>
      <Footer />
    </div>
  );
}
