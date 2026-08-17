import { create } from "zustand";
import { api } from "../api/client";

interface AuthState {
  token: string | null;
  email: string | null;
  isAuthenticated: boolean;
  login: (email: string, senha: string) => Promise<void>;
  registrar: (email: string, senha: string) => Promise<void>;
  logout: () => void;
}

export const useAuthStore = create<AuthState>((set) => ({
  token: localStorage.getItem("sc_token"),
  email: localStorage.getItem("sc_user"),
  isAuthenticated: !!localStorage.getItem("sc_token"),

  login: async (email, senha) => {
    const { data } = await api.post("/auth/login", { email, senha });
    localStorage.setItem("sc_token", data.token);
    localStorage.setItem("sc_user", data.email);
    set({ token: data.token, email: data.email, isAuthenticated: true });
  },

  registrar: async (email, senha) => {
    await api.post("/auth/registrar", { email, senha });
  },

  logout: () => {
    localStorage.removeItem("sc_token");
    localStorage.removeItem("sc_user");
    set({ token: null, email: null, isAuthenticated: false });
  },
}));
