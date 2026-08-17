import { defineStore } from "pinia";
import { ref, computed } from "vue";
import { api } from "../api/client";

export const useAuthStore = defineStore("auth", () => {
  const token = ref<string | null>(localStorage.getItem("sc_token"));
  const email = ref<string | null>(localStorage.getItem("sc_user"));
  const isAuthenticated = computed(() => !!token.value);

  async function login(loginEmail: string, senha: string) {
    const { data } = await api.post("/auth/login", { email: loginEmail, senha });
    localStorage.setItem("sc_token", data.token);
    localStorage.setItem("sc_user", data.email);
    token.value = data.token;
    email.value = data.email;
  }

  async function registrar(registerEmail: string, senha: string) {
    await api.post("/auth/registrar", { email: registerEmail, senha });
  }

  function logout() {
    localStorage.removeItem("sc_token");
    localStorage.removeItem("sc_user");
    token.value = null;
    email.value = null;
  }

  return { token, email, isAuthenticated, login, registrar, logout };
});
