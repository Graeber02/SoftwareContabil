# Software Contábil — Frontend (React + Ant Design 5)

Interface web do sistema de gestão contábil, migrada das telas JSF originais
para **React 18 + TypeScript + Ant Design 5**, consumindo a API .NET via REST.

## Stack

- **React 18** + **TypeScript** + **Vite**
- **Ant Design 5** (componentes de UI)
- **React Router 6** (rotas)
- **Zustand** (estado de autenticação)
- **Axios** (cliente HTTP, com interceptor JWT)
- **Day.js** (datas)

## Como rodar

```bash
npm install
cp .env.example .env   # ajuste VITE_API_URL se a API não estiver em localhost:5000
npm run dev
```

A aplicação sobe em `http://localhost:5173`. Crie um usuário pela tela de
login ("Criar conta") — ele é gravado via `POST /api/auth/registrar` no backend.

## Estrutura

```
src/
  api/
    client.ts          -> instância Axios + interceptor JWT
    crudFactory.ts      -> fábrica de chamadas REST genéricas (list/get/create/update/remove)
    modules.ts           -> APIs específicas dos módulos com regra de negócio
  components/
    GenericCrudPage.tsx  -> tela CRUD genérica (tabela + formulário) usada pelos cadastros simples
    EmpresaSelect.tsx     -> seletor de cliente/empresa (cliforid), reutilizado nas telas de negócio
  layouts/
    MainLayout.tsx       -> layout com menu lateral (Ant Design Sider/Menu)
  modules/
    cadastroModules.ts   -> metadados (campos, FKs) de cada cadastro simples — gerados a partir do modelo do backend
  routes/
    LoginPage, DashboardPage, ContasPagarPage, ContasReceberPage,
    PlanoDeContasPage, LancamentosContabeisPage, DrePage, BalancetePage,
    CompraPage / VendaPage (via MovimentacaoPage), PatrimonioPage
  store/
    authStore.ts         -> estado de autenticação (zustand)
```

## Como os cadastros simples funcionam

Os ~23 cadastros mais simples (Cidade, Estado, Marca, Unidade de Medida,
Local, Grupo, Espécie, Centro de Custo, etc.) não têm uma tela dedicada —
eles usam a rota genérica:

```
/cadastro/:moduleKey
```

renderizada por `GenericCrudPage.tsx`, que lê os metadados de
`modules/cadastroModules.ts` (campos, tipos, e quais são chaves estrangeiras)
e monta automaticamente a tabela e o formulário, incluindo os `<Select>` das
FKs (populados a partir do endpoint correspondente no backend).

Isso significa que, se um novo cadastro simples for adicionado no backend, ele
pode ganhar tela no frontend só adicionando uma entrada em
`cadastroModules.ts` — sem escrever componente novo.

## Telas com regra de negócio própria

Os módulos com fluxo específico têm página dedicada, que chama os endpoints
correspondentes do backend (ver `api/modules.ts`):

| Tela | Rota | Endpoint principal |
|---|---|---|
| Contas a Pagar | `/financeiro/contas-pagar` | `POST /contapagar/{id}/pagar` |
| Contas a Receber | `/financeiro/contas-receber` | `POST /contareceber/{id}/receber` |
| Plano de Contas | `/contabil/plano-de-contas` | `GET /conta/arvore/{cliforId}` |
| Lançamentos Contábeis | `/contabil/lancamentos` | CRUD de `LancamentoContabil` |
| DRE | `/contabil/dre` | `GET /gerencial/dre/{cliforId}` |
| Balancete | `/contabil/balancete` | `GET /gerencial/balancete/{cliforId}` |
| Compra / Venda | `/compra-venda/compra` e `/venda` | `POST /movimentacao/completa` |
| Patrimônio | `/patrimonio/bens` | `POST /patrimonio/{id}/baixar` |

## Build de produção

```bash
npm run build
```

Gera os arquivos estáticos em `dist/`, prontos para publicar em qualquer
servidor web (Nginx, Azure Static Web Apps, Vercel, etc.). Lembre de apontar
`VITE_API_URL` para a URL pública da API no ambiente de build.
