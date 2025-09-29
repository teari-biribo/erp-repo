import { useState } from "react";
import apiClient from "../apiclient"; // ✅ reuse your axios client

export default function OnboardingForm() {
  const userId = localStorage.getItem("userId");
  const [form, setForm] = useState({
    fullName: "",
    phone: "",
    position: "",
    city: "",
    state: "",
    postalCode: "",
    address: "",
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!userId) {
      alert("User not logged in.");
      return;
    }

    try {
      await apiClient.post("/onboarding/submit", { id: userId, ...form });
      alert("Onboarding form submitted!");
    } catch (err: any) {
      alert(err?.response?.data?.message ?? "Error submitting form.");
    }
  };

  return (
    <form onSubmit={handleSubmit} style={{ maxWidth: 500, margin: "40px auto" }}>
      <h2>New Employee Onboarding</h2>

      <input type="text" name="fullName" placeholder="Full Name"
        value={form.fullName} onChange={handleChange} required />
      <input type="text" name="phone" placeholder="Phone"
        value={form.phone} onChange={handleChange} required />
      <input type="text" name="position" placeholder="Position"
        value={form.position} onChange={handleChange} required />
      <input type="text" name="city" placeholder="City"
        value={form.city} onChange={handleChange} />
      <input type="text" name="state" placeholder="State"
        value={form.state} onChange={handleChange} />
      <input type="text" name="postalCode" placeholder="Postal Code"
        value={form.postalCode} onChange={handleChange} />
      <input type="text" name="address" placeholder="Address"
        value={form.address} onChange={handleChange} />

      <button type="submit">Submit</button>
    </form>
  );
}
