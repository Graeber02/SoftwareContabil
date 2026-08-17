import { api } from "./client";
import { createCrudApi } from "./crudFactory";

// ---------- Cadastros com tela própria ----------
export const usuarioApi = createCrudApi("usuario");
export const contaApi = {
  ...createCrudApi("conta"),
  arvore: (cliforId: string) => api.get(`/conta/arvore/${cliforId}`).then((r) => r.data),
};
export const lancamentoContabilApi = createCrudApi("lancamentocontabil");
export const movItensApi = createCrudApi("movitens");
export const baixaBemApi = createCrudApi("baixabem");
export const relatorioApi = createCrudApi("relatorio");
export const filtroRelatorioApi = createCrudApi("filtrorelatorio");
export const solicitacaoRelatorioApi = createCrudApi("solicitacaorelatorio");
export const auditoriaApi = createCrudApi("auditoria");

// ---------- Financeiro ----------
export const contaPagarApi = {
  ...createCrudApi("contapagar"),
  emAberto: (cliforId: string) => api.get(`/contapagar/em-aberto/${cliforId}`).then((r) => r.data),
  pagar: (id: number, payload: {
    valorPago: number; dataPagamento: string; descricao?: string;
    especieId: number; contaCorrenteId: number;
  }) => api.post(`/contapagar/${id}/pagar`, payload).then((r) => r.data),
};

export const contaReceberApi = {
  ...createCrudApi("contareceber"),
  emAberto: (cliforId: string) => api.get(`/contareceber/em-aberto/${cliforId}`).then((r) => r.data),
  receber: (id: number, payload: {
    valorRecebido: number; dataRecebimento: string; descricao?: string;
    especieId: number; contaCorrenteId: number;
  }) => api.post(`/contareceber/${id}/receber`, payload).then((r) => r.data),
};

// ---------- Compra e Venda ----------
export interface MovItemPayload {
  produtoId: number;
  localId: number;
  quantidade: number;
  valor: number;
}
export const movimentacaoApi = {
  ...createCrudApi("movimentacao"),
  itens: (id: number) => api.get(`/movimentacao/${id}/itens`).then((r) => r.data),
  criarCompleta: (payload: {
    notaFiscal?: string; tipo: "C" | "V"; data: string;
    cliForId: string; empresaId: string; itens: MovItemPayload[];
  }) => api.post("/movimentacao/completa", payload).then((r) => r.data),
};

// ---------- Patrimônio ----------
export const patrimonioApi = {
  ...createCrudApi("patrimonio"),
  ativos: (cliforId: string) => api.get(`/patrimonio/ativos/${cliforId}`).then((r) => r.data),
  baixar: (id: number, payload: {
    data: string; valor: number; observacao?: string; motivoBaixaId: number;
  }) => api.post(`/patrimonio/${id}/baixar`, payload).then((r) => r.data),
};

// ---------- Gerencial ----------
export const gerencialApi = {
  dre: (cliforId: string) => api.get(`/gerencial/dre/${cliforId}`).then((r) => r.data),
  balancete: (cliforId: string) => api.get(`/gerencial/balancete/${cliforId}`).then((r) => r.data),
  contasPagarReceber: (cliforId: string) =>
    api.get(`/gerencial/contas-pagar-receber/${cliforId}`).then((r) => r.data),
};
