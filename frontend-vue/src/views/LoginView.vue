<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import { message } from "ant-design-vue";
import { UserOutlined, LockOutlined } from "@ant-design/icons-vue";
import { useAuthStore } from "../stores/authStore";

const auth = useAuthStore();
const router = useRouter();

const mode = ref<"login" | "registrar">("login");
const loading = ref(false);
const form = ref({ email: "", senha: "" });

async function onFinish() {
  loading.value = true;
  try {
    if (mode.value === "login") {
      await auth.login(form.value.email, form.value.senha);
      router.push("/");
    } else {
      await auth.registrar(form.value.email, form.value.senha);
      message.success("Usuário criado! Faça login.");
      mode.value = "login";
    }
  } catch {
    message.error(mode.value === "login" ? "Email ou senha inválidos." : "Não foi possível registrar.");
  } finally {
    loading.value = false;
  }
}
</script>

<template>
  <div
    style="min-height: 100vh; display: flex; align-items: center; justify-content: center; background: linear-gradient(135deg,#0f2445,#173a68)"
  >
    <a-card style="width: 380px">
      <div style="text-align: center; margin-bottom: 16px">
        <h2 style="margin-bottom: 0">Software Contábil</h2>
        <span style="color: rgba(0,0,0,0.45)">Gestão financeira, patrimonial e contábil (Vue)</span>
      </div>
      <a-tabs v-model:activeKey="mode" centered>
        <a-tab-pane key="login" tab="Entrar" />
        <a-tab-pane key="registrar" tab="Criar conta" />
      </a-tabs>
      <a-form layout="vertical" @finish="onFinish">
        <a-form-item label="Email" required>
          <a-input v-model:value="form.email" placeholder="voce@empresa.com">
            <template #prefix><user-outlined /></template>
          </a-input>
        </a-form-item>
        <a-form-item label="Senha" required>
          <a-input-password v-model:value="form.senha" placeholder="••••••••">
            <template #prefix><lock-outlined /></template>
          </a-input-password>
        </a-form-item>
        <a-form-item>
          <a-button type="primary" html-type="submit" block :loading="loading">
            {{ mode === "login" ? "Entrar" : "Criar conta" }}
          </a-button>
        </a-form-item>
      </a-form>
    </a-card>
  </div>
</template>
