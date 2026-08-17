import { BrowserRouter, Routes, Route } from "react-router-dom";
import MainLayout from "./layouts/MainLayout";
import ProtectedRoute from "./routes/ProtectedRoute";
import LoginPage from "./routes/LoginPage";
import DashboardPage from "./routes/DashboardPage";
import ContasPagarPage from "./routes/ContasPagarPage";
import ContasReceberPage from "./routes/ContasReceberPage";
import PlanoDeContasPage from "./routes/PlanoDeContasPage";
import LancamentosContabeisPage from "./routes/LancamentosContabeisPage";
import DrePage from "./routes/DrePage";
import BalancetePage from "./routes/BalancetePage";
import CompraPage from "./routes/CompraPage";
import VendaPage from "./routes/VendaPage";
import PatrimonioPage from "./routes/PatrimonioPage";
import GenericCrudPage from "./components/GenericCrudPage";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />

        <Route element={<ProtectedRoute />}>
          <Route element={<MainLayout />}>
            <Route path="/" element={<DashboardPage />} />

            <Route path="/financeiro/contas-pagar" element={<ContasPagarPage />} />
            <Route path="/financeiro/contas-receber" element={<ContasReceberPage />} />

            <Route path="/contabil/plano-de-contas" element={<PlanoDeContasPage />} />
            <Route path="/contabil/lancamentos" element={<LancamentosContabeisPage />} />
            <Route path="/contabil/dre" element={<DrePage />} />
            <Route path="/contabil/balancete" element={<BalancetePage />} />

            <Route path="/compra-venda/compra" element={<CompraPage />} />
            <Route path="/compra-venda/venda" element={<VendaPage />} />

            <Route path="/patrimonio/bens" element={<PatrimonioPage />} />

            <Route path="/cadastro/:moduleKey" element={<GenericCrudPage />} />
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
