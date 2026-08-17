# Software Contábil — Backend (.NET 10)

API REST do sistema de gestão contábil, organizada em **Clean Architecture**
(Domain / Application / Infrastructure / Api), com **cada entidade tendo seu
próprio repositório, serviço e controller explícitos** — sem abstração
genérica de CRUD (sem `IRepository<TEntity,TKey>`, sem `ICrudService<TEntity,TKey>`,
sem controller base genérica).

## Por que sem CRUD genérico

Em vez de uma classe/interface reutilizável que serve para qualquer entidade
via generics, todo módulo declara explicitamente os métodos que expõe:

```csharp
// Domain/Repositories/ICidadeRepository.cs
public interface ICidadeRepository
{
    Task<IReadOnlyList<Cidade>> GetAllAsync(CancellationToken ct = default);
    Task<Cidade?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Cidade> AddAsync(Cidade entity, CancellationToken ct = default);
    Task UpdateAsync(Cidade entity, CancellationToken ct = default);
    Task DeleteAsync(Cidade entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}
```

O mesmo padrão se repete em `Infrastructure/Repositories/CidadeRepository.cs`
(implementação EF Core própria da entidade), `Application/.../CidadeService.cs`
(regra de aplicação própria) e `Api/Controllers/CidadeController.cs` (endpoints
REST próprios) — nenhum deles herda de uma base genérica.

Módulos com regra de negócio (Contas a Pagar/Receber, Patrimônio, Movimentação,
Plano de Contas) seguem o mesmo princípio: o CRUD básico é declarado
explicitamente na própria interface do serviço, ao lado dos métodos de regra
de negócio (`PagarAsync`, `BaixarAsync`, `CriarCompletaAsync`...).

## Arquitetura em camadas

```
src/
  SoftwareContabil.Domain           -> entidades, contratos (uma interface de repositório por entidade), exceções
  SoftwareContabil.Application      -> um serviço por entidade (CRUD explícito + regra de negócio)
  SoftwareContabil.Infrastructure   -> um repositório por entidade (implementação EF Core/Dapper)
  SoftwareContabil.Api              -> uma controller por entidade (endpoints REST explícitos)
```

A regra de dependência continua de fora para dentro:

```
Api  ──depende de──>  Application  ──depende de──>  Domain
Infrastructure  ──implementa contratos de──>  Domain / Application
```

## Estrutura por entidade (exemplo: Cidade)

| Camada | Arquivo | Conteúdo |
|---|---|---|
| Domain | `Repositories/ICidadeRepository.cs` | Contrato: `GetAllAsync`, `GetByIdAsync`, `AddAsync`, `UpdateAsync`, `DeleteAsync`, `ExistsAsync` |
| Infrastructure | `Repositories/CidadeRepository.cs` | Implementação EF Core, usando `DbSet<Cidade>` diretamente |
| Application | `Cadastros/CidadeService.cs` | `ICidadeService` + `CidadeService`, validando Id e traduzindo "não encontrado" para `NotFoundException` |
| Api | `Controllers/CidadeController.cs` | 5 endpoints REST (`GET`, `GET/{id}`, `POST`, `PUT/{id}`, `DELETE/{id}`), chamando `ICidadeService` |

Esse padrão se repete para as 36 entidades. Módulos de negócio adicionam
métodos extras nesses mesmos arquivos (ex.: `IContaPagarService` tem os 5
métodos de CRUD **e** `GetEmAbertoAsync`/`PagarAsync`).

## Injeção de dependência

Como não há generics, cada repositório e cada serviço é registrado
individualmente em `DependencyInjection.cs` (`Infrastructure` e `Application`,
respectivamente):

```csharp
// Infrastructure/DependencyInjection.cs
services.AddScoped<ICidadeRepository, CidadeRepository>();
services.AddScoped<IEstadoRepository, EstadoRepository>();
// ... uma linha por entidade (37 no total, incluindo o repositório gerencial)

// Application/DependencyInjection.cs
services.AddScoped<ICidadeService, CidadeService>();
services.AddScoped<IEstadoService, EstadoService>();
// ... uma linha por entidade
```

## Como rodar

### 1. Banco de dados

```bash
docker compose up -d
```

Isso sobe um Postgres 16 em `localhost:5432` já com o schema (`db/001_schema.sql`)
e as funções de negócio (`db/002_functions.sql`) aplicadas automaticamente.

### 2. Backend

```bash
cd src/SoftwareContabil.Api
dotnet restore
dotnet run
```

API em `http://localhost:5000`, Swagger/OpenAPI em `http://localhost:5000/swagger`.

### 3. Primeiro usuário

```
POST /api/auth/registrar
{ "email": "admin@empresa.com", "senha": "SenhaForte123" }
```

Depois `POST /api/auth/login` para obter o token JWT.

## Módulos principais

| Módulo | Controller | Serviço | Repositório | Observações |
|---|---|---|---|---|
| Cadastros gerais/produto | 29 controllers explícitas | `I{Nome}Service` | `I{Nome}Repository` | CRUD explícito, sem herança |
| Financeiro | `ContaPagarController`, `ContaReceberController` | `IContaPagarService`, `IContaReceberService` | `IContaPagarRepository`, `IContaReceberRepository` | `POST /{id}/pagar` e `POST /{id}/receber` |
| Contábil | `ContaController` | `IContaService` | `IContaRepository` | `GET /api/conta/arvore/{cliforId}` |
| Compra e Venda | `MovimentacaoController` | `IMovimentacaoService` | `IMovimentacaoRepository` | `POST /api/movimentacao/completa` (transação com itens + estoque) |
| Patrimônio | `PatrimonioController` | `IPatrimonioService` | `IPatrimonioRepository` | `POST /api/patrimonio/{id}/baixar` |
| Gerencial | `GerencialController` | `IGerencialService` | `IGerencialRepository` | DRE, balancete (via Dapper) |
| Segurança | `AuthController` | `IAuthService` | `IUsuarioRepository` | JWT + BCrypt por trás de interfaces |

## Melhorias aplicadas

Além da arquitetura em camadas sem CRUD genérico, esta versão inclui:

- **Segurança**
  - Validação da chave JWT na inicialização (falha rápido se `Jwt:Key` tiver menos de 32 caracteres, em vez de erro críptico em runtime).
  - Rate limiting no login/registro (`AuthController`) — 10 requisições/minuto por padrão, protegendo contra força bruta.
  - Política de senha também validada no backend (mínimo 6 caracteres), não só no frontend.
  - CORS configurável com múltiplas origens (`Frontend:Urls`, separadas por vírgula) — útil com mais de um frontend rodando ao mesmo tempo (React + Vue, por exemplo).
- **Observabilidade**
  - Health check em `GET /health`, verificando conectividade com o Postgres.
  - Logging estruturado (`ILogger<T>`) nos serviços de negócio (pagamento, recebimento, baixa de bem, movimentação, login/registro) — sem nunca logar senha em texto claro.
- **Validação de dados**
  - Data Annotations nas entidades mais críticas (`ContaPagar`, `ContaReceber`, `Patrimonio`, `Conta`, `CliFor`, `Produto`) — o `[ApiController]` já devolve `400` automaticamente para dados inválidos, sem precisar de código extra no controller.
  - Regras de negócio adicionais nos serviços: vencimento não pode ser anterior à emissão do documento (contas a pagar/receber), saldo do título sempre nasce igual ao valor (não dá pra criar um título "já parcialmente pago" por fora do fluxo de pagamento), item de movimentação não pode ter quantidade ≤ 0 ou valor negativo, bem patrimonial sempre nasce ativo (`baixado = 0`).
- **Performance/escala**
  - Paginação explícita (`GET /paginado?page=&pageSize=`) nas entidades de maior volume: `CliFor`, `Produto`, `LancamentoContabil`, `Movimentacao`. O endpoint `GetAll` sem paginação continua funcionando (compatibilidade com o frontend existente) — a paginação é uma rota adicional, não uma quebra de contrato.

## Observações da migração

- Os relatórios originais em **JasperReports (.jrxml)** não foram portados; os
  dados ficam expostos via `GerencialController` para o frontend renderizar/exportar.
- As funções PL/pgSQL (`calcula_dre`, `hierarquia`, `contas_filhas`) continuam
  no banco (`db/002_functions.sql`), acessadas via Dapper.
- Não há `dotnet` SDK disponível no ambiente onde este projeto foi gerado —
  o código foi revisado manualmente. O projeto já foi compilado com sucesso
  localmente (evidenciado pelos artefatos de build) antes desta rodada de
  melhorias; rode `dotnet build` para confirmar após aplicar as mudanças.
