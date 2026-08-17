import { createRouter, createWebHistory } from "vue-router";
import { useAuthStore } from "../stores/authStore";
import MainLayout from "../layouts/MainLayout.vue";
import LoginView from "../views/LoginView.vue";
import DashboardView from "../views/DashboardView.vue";
import ContasPagarView from "../views/ContasPagarView.vue";
import ContasReceberView from "../views/ContasReceberView.vue";
import PlanoDeContasView from "../views/PlanoDeContasView.vue";
import LancamentosContabeisView from "../views/LancamentosContabeisView.vue";
import DreView from "../views/DreView.vue";
import BalanceteView from "../views/BalanceteView.vue";
import CompraView from "../views/CompraView.vue";
import VendaView from "../views/VendaView.vue";
import PatrimonioView from "../views/PatrimonioView.vue";
import GenericCrudView from "../views/GenericCrudView.vue";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/login", name: "login", component: LoginView },
    {
      path: "/",
      component: MainLayout,
      meta: { requiresAuth: true },
      children: [
        { path: "", name: "dashboard", component: DashboardView },
        { path: "financeiro/contas-pagar", name: "contas-pagar", component: ContasPagarView },
        { path: "financeiro/contas-receber", name: "contas-receber", component: ContasReceberView },
        { path: "contabil/plano-de-contas", name: "plano-de-contas", component: PlanoDeContasView },
        { path: "contabil/lancamentos", name: "lancamentos", component: LancamentosContabeisView },
        { path: "contabil/dre", name: "dre", component: DreView },
        { path: "contabil/balancete", name: "balancete", component: BalanceteView },
        { path: "compra-venda/compra", name: "compra", component: CompraView },
        { path: "compra-venda/venda", name: "venda", component: VendaView },
        { path: "patrimonio/bens", name: "patrimonio", component: PatrimonioView },
        { path: "cadastro/:moduleKey", name: "cadastro-generico", component: GenericCrudView, props: true },
      ],
    },
  ],
});

router.beforeEach((to) => {
  const auth = useAuthStore();
  if (to.meta.requiresAuth && !auth.isAuthenticated) {
    return { name: "login" };
  }
  return true;
});

export default router;
