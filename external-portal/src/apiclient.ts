import axios from "axios";

// Base configuration for all API requests
const apiClient = axios.create({
  baseURL: "http://localhost:5000/api", // backend API base URL
  headers: {
    "Content-Type": "application/json",
  },
});

// ✅ Interceptor: Automatically attach token (if available)
apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default apiClient;
