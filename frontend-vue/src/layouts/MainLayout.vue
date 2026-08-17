<script setup lang="ts">
import { computed, h, ref } from "vue";
import { useRouter, useRoute } from "vue-router";
import {
  DashboardOutlined,
  BankOutlined,
  TeamOutlined,
  ShoppingOutlined,
  SwapOutlined,
  HomeOutlined,
  BarChartOutlined,
  SettingOutlined,
  LogoutOutlined,
  UserOutlined,
} from "@ant-design/icons-vue";
import { useAuthStore } from "../stores/authStore";
import { CADASTRO_MODULES } from "../modules/cadastroModules";

const router = useRouter();
const route = useRoute();
const auth = useAuthStore();
const collapsed = ref(false);

const groupIcon: Record<string, any> = {
  "Cadastros Gerais": TeamOutlined,
  "Cadastros de Produtos": ShoppingOutlined,
  Financeiro: BankOutlined,
  "Patrimônio": HomeOutlined,
  Gerencial: BarChartOutlined,
  Sistema: SettingOutlined,
};

const menuItems = computed(() => {
  const byGroup: Record<string, { key: string; label: string }[]> = {};
  for (const m of CADASTRO_MODULES) {
    byGroup[m.group] = byGroup[m.group] ?? [];
    byGroup[m.group].push({ key: `/cadastro/${m.key}`, label: m.label });
  }

  const cadastroChildren = Object.entries(byGroup).map(([group, items]) => ({
    key: group,
    icon: () => h(groupIcon[group] ?? SettingOutlined),
    label: group,
    children: items,
  }));

  return [
    { key: "/", icon: () => h(DashboardOutlined), label: "Início" },
    {
      key: "financeiro",
      icon: () => h(BankOutlined),
      label: "Financeiro",
      children: [
        { key: "/financeiro/contas-pagar", label: "Contas a Pagar" },
        { key: "/financeiro/contas-receber", label: "Contas a Receber" },
      ],
    },
    {
      key: "contabil",
      icon: () => h(BarChartOutlined),
      label: "Contábil",
      children: [
        { key: "/contabil/plano-de-contas", label: "Plano de Contas" },
        { key: "/contabil/lancamentos", label: "Lançamentos Contábeis" },
        { key: "/contabil/dre", label: "DRE" },
        { key: "/contabil/balancete", label: "Balancete" },
      ],
    },
    {
      key: "compra-venda",
      icon: () => h(SwapOutlined),
      label: "Compra e Venda",
      children: [
        { key: "/compra-venda/compra", label: "Compra" },
        { key: "/compra-venda/venda", label: "Venda" },
      ],
    },
    {
      key: "patrimonio",
      icon: () => h(HomeOutlined),
      label: "Patrimônio",
      children: [{ key: "/patrimonio/bens", label: "Bens Patrimoniais" }],
    },
    ...cadastroChildren,
  ];
});

const selectedKeys = computed(() => [route.path]);

function onMenuClick({ key }: { key: string }) {
  if (typeof key === "string" && key.startsWith("/")) router.push(key);
}

function handleLogout() {
  auth.logout();
  router.push("/login");
}
</script>

<template>
  <a-layout style="min-height: 100vh">
    <a-layout-sider v-model:collapsed="collapsed" collapsible :width="260">
      <div
        :style="{
          height: '56px', margin: '12px', color: '#fff', fontWeight: 700,
          fontSize: collapsed ? '16px' : '18px', textAlign: 'center',
          lineHeight: '56px', overflow: 'hidden', whiteSpace: 'nowrap',
        }"
      >
        {{ collapsed ? "SC" : "Software Contábil" }}
      </div>
      <a-menu theme="dark" mode="inline" :selected-keys="selectedKeys" :items="menuItems" @click="onMenuClick" />
    </a-layout-sider>
    <a-layout>
      <a-layout-header
        style="padding: 0 24px; background: #fff; display: flex; align-items: center; justify-content: flex-end; border-bottom: 1px solid #f0f0f0"
      >
        <a-dropdown placement="bottomRight">
          <div style="display: flex; align-items: center; gap: 8px; cursor: pointer">
            <a-avatar :icon="h(UserOutlined)" />
            <span>{{ auth.email }}</span>
          </div>
          <template #overlay>
            <a-menu @click="handleLogout">
              <a-menu-item key="logout">
                <logout-outlined /> Sair
              </a-menu-item>
            </a-menu>
          </template>
        </a-dropdown>
      </a-layout-header>
      <a-layout-content style="margin: 24px">
        <RouterView />
      </a-layout-content>
    </a-layout>
  </a-layout>
</template>
