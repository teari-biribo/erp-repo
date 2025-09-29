import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import apiClient from "../apiclient";

export default function LoginAndReset() {
  const navigate = useNavigate();
  const [mode, setMode] = useState<"login" | "reset">("login");

  // shared
  const [email, setEmail] = useState("");

  // login
  const [password, setPassword] = useState("");
  const [showPwd, setShowPwd] = useState(false);
  const [loginMessage, setLoginMessage] = useState("");
  const [loginError, setLoginError] = useState(false);

  // reset
  const [newPwd, setNewPwd] = useState("");
  const [confirmPwd, setConfirmPwd] = useState("");
  const [showNew, setShowNew] = useState(false);
  const [showConfirm, setShowConfirm] = useState(false);
  const [resetMessage, setResetMessage] = useState("");
  const [resetError, setResetError] = useState(false);

  // ✅ Login
  const doLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoginMessage("");
    setLoginError(false);

    try {
      const res = await apiClient.post("/auth/login", { email, password });
      if (res.data.success) {
        localStorage.setItem("userId", res.data.userId);
        setLoginMessage(res.data.message || "Login successful ✅ Redirecting...");
        setTimeout(() => navigate("/onboarding"), 1000);
      } else {
        setLoginMessage(res.data.message || "Login failed.");
        setLoginError(true);
      }
    } catch (err: any) {
      setLoginMessage(err?.response?.data?.message ?? "Login failed.");
      setLoginError(true);
    }
  };

  // ✅ Reset password
  const doReset = async (e: React.FormEvent) => {
    e.preventDefault();
    setResetMessage("");
    setResetError(false);

    if (newPwd !== confirmPwd) {
      setResetMessage("Passwords do not match.");
      setResetError(true);
      return;
    }

    try {
      const res = await apiClient.post("/auth/forgot-password", {
        email,
        newPassword: newPwd,
      });

      if (res.data.success) {
        setResetMessage(res.data.message || "Password updated. Please login again.");
        setResetError(false);

        // After password reset → go back to login with new password filled
        setPassword(newPwd);
        setMode("login");
      } else {
        setResetMessage(res.data.message || "User not found.");
        setResetError(true);
      }
    } catch (err: any) {
      setResetMessage(err?.response?.data?.message ?? "Error resetting password.");
      setResetError(true);
    }
  };

  return (
    <div style={{ maxWidth: 420, margin: "60px auto" }}>
      <h2>Candidate Portal</h2>

      {/* ✅ LOGIN FORM */}
      {mode === "login" && (
        <form onSubmit={doLogin}>
          <input
            type="email"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            style={{ display: "block", width: "100%", marginBottom: 10 }}
          />

          <div style={{ position: "relative", marginBottom: 10 }}>
            <input
              type={showPwd ? "text" : "password"}
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              style={{ display: "block", width: "100%" }}
            />
            <button
              type="button"
              onClick={() => setShowPwd(!showPwd)}
              style={{
                position: "absolute",
                right: 10,
                top: "50%",
                transform: "translateY(-50%)",
                border: "none",
                background: "none",
                cursor: "pointer",
              }}
            >
              {showPwd ? "Hide" : "Show"}
            </button>
          </div>

          {loginMessage && (
            <div style={{ color: loginError ? "red" : "green", marginBottom: 10 }}>
              {loginMessage}
            </div>
          )}

          <button type="submit">Sign in</button>
          <div style={{ marginTop: 10 }}>
            <a href="#" onClick={() => setMode("reset")}>
              Forgot password?
            </a>
          </div>
        </form>
      )}

      {/* ✅ RESET FORM */}
      {mode === "reset" && (
        <form onSubmit={doReset}>
          <h3>Reset Password</h3>
          <input
            type="email"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            style={{ display: "block", width: "100%", marginBottom: 10 }}
          />

          <div style={{ position: "relative", marginBottom: 10 }}>
            <input
              type={showNew ? "text" : "password"}
              placeholder="New password"
              value={newPwd}
              onChange={(e) => setNewPwd(e.target.value)}
              required
              style={{ display: "block", width: "100%" }}
            />
            <button
              type="button"
              onClick={() => setShowNew(!showNew)}
              style={{
                position: "absolute",
                right: 10,
                top: "50%",
                transform: "translateY(-50%)",
                border: "none",
                background: "none",
                cursor: "pointer",
              }}
            >
              {showNew ? "Hide" : "Show"}
            </button>
          </div>

          <div style={{ position: "relative", marginBottom: 10 }}>
            <input
              type={showConfirm ? "text" : "password"}
              placeholder="Confirm password"
              value={confirmPwd}
              onChange={(e) => setConfirmPwd(e.target.value)}
              required
              style={{ display: "block", width: "100%" }}
            />
            <button
              type="button"
              onClick={() => setShowConfirm(!showConfirm)}
              style={{
                position: "absolute",
                right: 10,
                top: "50%",
                transform: "translateY(-50%)",
                border: "none",
                background: "none",
                cursor: "pointer",
              }}
            >
              {showConfirm ? "Hide" : "Show"}
            </button>
          </div>

          {resetMessage && (
            <div style={{ color: resetError ? "red" : "green", marginBottom: 10 }}>
              {resetMessage}
            </div>
          )}

          <button type="submit">Submit</button>
          <div style={{ marginTop: 10 }}>
            <a href="#" onClick={() => setMode("login")}>
              Back to login
            </a>
          </div>
        </form>
      )}
    </div>
  );
}
