# Software Contábil — Frontend (Vue 3 + Ant Design Vue)

Segunda implementação de interface web do sistema, em **Vue 3 + TypeScript +
Ant Design Vue**, consumindo a **mesma API REST** usada pelo frontend React
(`../frontend`). As duas interfaces são intercambiáveis — não há nenhuma
dependência entre elas, e você pode rodar qualquer uma (ou as duas ao mesmo
tempo, em portas diferentes) contra o mesmo backend.

## Stack

- **Vue 3** (Composition API, `<script setup>`) + **TypeScript** + **Vite**
- **Ant Design Vue 4** (componentes de UI — equivalente ao Ant Design 5 usado no React)
- **Vue Router 4** (rotas, com guarda de autenticação)
- **Pinia** (estado de autenticação — equivalente ao Zustand usado no React)
- **Axios** (cliente HTTP, com interceptor JWT)
- **Day.js** (datas)

## Como rodar

```bash
npm install
cp .env.example .env   # ajuste VITE_API_URL se a API não estiver em localhost:5000
npm run dev
```

A aplicação sobe em `http://localhost:5174` (porta diferente do frontend
React, que usa 5173 — assim dá para rodar os dois ao mesmo tempo). O backend
já vem configurado (`Frontend:Urls` no `appsettings.json`) para aceitar
requisições de ambas as portas.

## O que foi reaproveitado do frontend React

Estes quatro arquivos são **100% agnósticos de framework** (TypeScript puro,
sem JSX/imports de React) e foram copiados sem nenhuma alteração:

```
src/api/client.ts             -> instância Axios + interceptor JWT
src/api/crudFactory.ts        -> fábrica de chamadas REST genéricas
src/api/modules.ts            -> APIs específicas dos módulos com regra de negócio
src/modules/cadastroModules.ts -> metadados dos 29 cadastros simples
```

Isso significa que qualquer mudança de contrato com a API (novo campo, nova
rota) só precisa ser replicada nesses 4 arquivos — o resto (componentes Vue)
não muda.

## Estrutura

```
src/
  api/            -> ver acima (compartilhado com o React)
  components/
    EmpresaSelect.vue   -> seletor de cliente/empresa (cliforid)
  layouts/
    MainLayout.vue      -> layout com menu lateral (Ant Design Vue Sider/Menu)
  modules/
    cadastroModules.ts  -> ver acima (compartilhado com o React)
  router/
    index.ts            -> rotas + guarda de autenticação (beforeEach)
  stores/
    authStore.ts         -> estado de autenticação (Pinia)
  views/
    LoginView, DashboardView, ContasPagarView, ContasReceberView,
    PlanoDeContasView, LancamentosContabeisView, DreView, BalanceteView,
    CompraView / VendaView (via MovimentacaoBase.vue), PatrimonioView,
    GenericCrudView (cadastros simples, dirigida por metadados)
```

## Cadastros simples (CRUD genérico dirigido por metadados)

Assim como no React, os ~29 cadastros mais simples (Cidade, Estado, Marca,
Unidade de Medida, Local, Grupo, Espécie, Centro de Custo etc.) não têm tela
dedicada — usam a rota genérica:

```
/cadastro/:moduleKey
```

renderizada por `GenericCrudView.vue`, que lê `modules/cadastroModules.ts`
(mesmo arquivo do React) e monta a tabela e o formulário automaticamente,
incluindo os `<a-select>` das chaves estrangeiras.

## Diferenças de implementação em relação ao React

| Conceito | React | Vue |
|---|---|---|
| Estado de autenticação | Zustand (`authStore.ts`) | Pinia (`authStore.ts`) |
| Rotas | React Router (`<Route>`) | Vue Router (`routes: []`) |
| Proteção de rota | `<ProtectedRoute>` (componente) | `router.beforeEach` (guarda global) |
| Formulário controlado | `useState` + `Form.useForm()` (AntD) | `ref()` + `v-model` |
| Ícones | `@ant-design/icons` | `@ant-design/icons-vue` |

O comportamento e os endpoints chamados são idênticos — só a forma de
escrever o componente muda.

## Build de produção

```bash
npm run build
```

Gera os arquivos estáticos em `dist/`, prontos para publicar em qualquer
servidor web.
