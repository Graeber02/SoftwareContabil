import { api } from "./client";

/**
 * Cria um conjunto padrão de chamadas REST (list/get/create/update/remove)
 * para um recurso, espelhando a CrudControllerBase do backend .NET.
 * Usado pelos cadastros simples; módulos com regra de negócio própria
 * (contaPagar.pagar, patrimonio.baixar, movimentacao.completa, etc.)
 * complementam com métodos extras — ver src/api/modules.ts.
 */
export function createCrudApi<T = any>(resource: string) {
  const base = `/${resource}`;
  return {
    list: () => api.get<T[]>(base).then((r) => r.data),
    get: (id: string | number) => api.get<T>(`${base}/${id}`).then((r) => r.data),
    create: (payload: Partial<T>) => api.post<T>(base, payload).then((r) => r.data),
    update: (id: string | number, payload: Partial<T>) =>
      api.put(`${base}/${id}`, payload).then((r) => r.data),
    remove: (id: string | number) => api.delete(`${base}/${id}`).then((r) => r.data),
  };
}
