# Software Contábil

Sistema de gestão contábil, financeira, patrimonial e de estoque — migrado do
projeto original (Java + Spring Boot + JSF + PrimeFaces) para uma stack
moderna, com o backend organizado em **Clean Architecture** (Domain /
Application / Infrastructure / Api), **sem abstração genérica de CRUD**
(cada entidade tem seu próprio repositório, serviço e controller explícitos),
e **dois frontends alternativos** consumindo a mesma API.

- **Backend:** .NET 10, 4 camadas, EF Core (escrita) + Dapper (leitura/relatórios), PostgreSQL
- **Frontend React:** React 18 + TypeScript + Ant Design 5 (porta 5173)
- **Frontend Vue:** Vue 3 + TypeScript + Ant Design Vue (porta 5174)
- **Banco de dados:** PostgreSQL (schema + funções de negócio incluídos)

## Estrutura do repositório

```
software-contabil/
  backend/
    src/
      SoftwareContabil.Domain          -> entidades, um contrato de repositório por entidade, exceções — zero dependências externas
      SoftwareContabil.Application     -> um serviço por entidade (CRUD explícito + regra de negócio)
      SoftwareContabil.Infrastructure  -> um repositório por entidade (EF Core/Dapper), JWT, BCrypt
      SoftwareContabil.Api             -> uma controller por entidade, Swagger, rate limiting, health check
    db/                                -> schema PostgreSQL + funções de negócio (DRE, hierarquia)
  frontend/                            -> SPA React + Ant Design 5 (porta 5173)
  frontend-vue/                        -> SPA Vue 3 + Ant Design Vue (porta 5174)
```

Cada pasta tem seu próprio `README.md` com instruções detalhadas — o do
backend (`backend/README.md`) explica por que não há CRUD genérico e onde
cada melhoria foi aplicada; os dos frontends explicam a estrutura de telas.

Os dois frontends são **intercambiáveis**: consomem exatamente a mesma API,
não dependem um do outro, e podem rodar ao mesmo tempo (o backend já aceita
CORS das duas portas por padrão).

## Subindo o projeto do zero

### 1. Banco de dados

```bash
cd backend
docker compose up -d
```

Isso cria o banco `software_contabil` no Postgres já com todas as tabelas e
funções aplicadas (`backend/db/001_schema.sql` e `002_functions.sql`).

### 2. Backend

```bash
cd backend/src/SoftwareContabil.Api
dotnet restore
dotnet run
```

API em `http://localhost:5000`. Documentação Swagger/OpenAPI em
`http://localhost:5000/swagger`, health check em `http://localhost:5000/health`.

### 3. Frontend (escolha um, ou rode os dois)

**React:**
```bash
cd frontend
npm install
cp .env.example .env
npm run dev   # http://localhost:5173
```

**Vue:**
```bash
cd frontend-vue
npm install
cp .env.example .env
npm run dev   # http://localhost:5174
```

### 4. Primeiro acesso

Abra o frontend escolhido, use a aba "Criar conta" para registrar um usuário
(mínimo 6 caracteres de senha, validado no backend), faça login e cadastre
pelo menos um **Cliente/Fornecedor** (módulo "Cadastros Gerais →
Cliente/Fornecedor") — é ele quem representa a empresa/escopo (`cliforid`)
usado nos módulos financeiro, contábil e patrimonial.

## Arquitetura do backend (resumo)

```
Api  ──depende de──>  Application  ──depende de──>  Domain
Infrastructure  ──implementa contratos de──>  Domain / Application
```

- **Sem CRUD genérico**: não existe `IRepository<TEntity,TKey>` nem
  `ICrudService<TEntity,TKey>` nem controller base genérica. Cada uma das 36
  entidades tem seu próprio `I{Nome}Repository`, `{Nome}Repository`,
  `I{Nome}Service`/`{Nome}Service` e `{Nome}Controller`, escritos de forma
  explícita — o CRUD básico é declarado método a método em cada interface.
- Controllers **não têm regra de negócio** — só traduzem HTTP e chamam o
  serviço correspondente (`IContaPagarService`, `IPatrimonioService` etc.).
- Módulos com regra própria (contas a pagar/receber, patrimônio, compra/venda,
  plano de contas) somam, na mesma interface do serviço, os métodos de CRUD
  **e** os de negócio (`PagarAsync`, `BaixarAsync`, `CriarCompletaAsync`...).
- Erros de negócio são exceções de domínio (`NotFoundException`,
  `BusinessRuleException`), traduzidas para HTTP em um único middleware.

Detalhes completos estão em `backend/README.md`.

## Melhorias de produção aplicadas

- **Segurança**: validação da chave JWT na inicialização, rate limiting no
  login/registro (força bruta), política de senha também no backend, CORS
  multi-origem configurável.
- **Observabilidade**: health check (`/health`), logging estruturado
  (`ILogger<T>`) nas operações de negócio (pagamento, recebimento, baixa,
  movimentação, login).
- **Validação de dados**: Data Annotations nas entidades mais críticas
  (`ContaPagar`, `ContaReceber`, `Patrimonio`, `Conta`, `CliFor`, `Produto`)
  e regras de negócio adicionais nos serviços (datas consistentes, saldo
  inicial correto, quantidades/valores não-negativos).
- **Escala**: paginação explícita (`GET /paginado?page=&pageSize=`) nas
  entidades de maior volume (`CliFor`, `Produto`, `LancamentoContabil`,
  `Movimentacao`), sem quebrar o `GetAll` já usado pelos frontends.

Lista completa em `backend/README.md`.

## O que foi migrado

| Área | Situação |
|---|---|
| Modelo de dados (36 entidades) | ✅ Migrado integralmente para Postgres/EF Core |
| Regras de negócio: contas a pagar/receber, baixa de bens, compra/venda com estoque | ✅ Reescritas em serviços da camada Application |
| Plano de contas hierárquico, DRE, balancete | ✅ Portado (funções PL/pgSQL originais mantidas + endpoints REST) |
| Autenticação | ✅ JWT + BCrypt por trás de interfaces (`IPasswordHasher`, `ITokenGenerator`), com rate limiting e política de senha |
| Telas (CRUD simples) | ✅ Tela genérica dirigida por metadados, implementada em **React** e em **Vue** |
| Telas de negócio (financeiro, patrimônio, compra/venda, contábil, gerencial) | ✅ Telas dedicadas, em **React** e em **Vue** |
| Relatórios em **JasperReports** (.jrxml) | ⚠️ Não portado — formato específico da stack Java. Dados disponíveis via `/api/gerencial/*` para o frontend exibir/exportar |
| Depreciação automática de bens (cálculo mensal) | ⚠️ Estrutura de dados migrada (`Depreciacao`), job/agendador não portado — pode virar um `BackgroundService` |

## Próximos passos sugeridos

1. Rodar `dotnet build` na solução como primeiro passo.
2. Rodar `dotnet ef migrations add InitialCreate` caso prefira gerenciar o
   schema via Migrations do EF Core em vez do SQL puro fornecido.
3. Adicionar exportação em PDF/Excel das telas de DRE/Balancete.
4. Implementar o `BackgroundService` de cálculo mensal de depreciação.
5. Escrever testes unitários para os serviços da Application (a separação em
   camadas já deixa isso simples: basta mockar as interfaces de repositório).
6. Estender Data Annotations e paginação para os demais cadastros simples,
   seguindo o mesmo padrão já aplicado aos 6 módulos mais críticos.
