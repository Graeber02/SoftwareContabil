using Microsoft.Extensions.DependencyInjection;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Application.Service;

namespace SoftwareContabil.Application;

/// <summary>
/// Ponto único de registro da camada de Aplicação: um serviço por entidade
/// (sem abstração genérica de CRUD) — a Api chama apenas <c>AddApplication()</c>.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Serviços de negócio (contas a pagar/receber, patrimônio, plano de contas,
        // compra/venda) — cada um com CRUD explícito + a regra própria do módulo.
        services.AddScoped<IContaPagarService, ContaPagarService>();
        services.AddScoped<IContaReceberService, ContaReceberService>();
        services.AddScoped<IPatrimonioService, PatrimonioService>();
        services.AddScoped<IMovimentacaoService, MovimentacaoService>();
        services.AddScoped<IContaService, ContaService>();
        services.AddScoped<IGerencialService, GerencialService>();
        services.AddScoped<IAuthService, AuthService>();

        // Serviços de CRUD explícito dos demais cadastros.
        services.AddScoped<IEstadoService, EstadoService>();
        services.AddScoped<ICidadeService, CidadeService>();
        services.AddScoped<IEnderecoService, EnderecoService>();
        services.AddScoped<ICliForService, CliForService>();
        services.AddScoped<ICentroCustoService, CentroCustoService>();
        services.AddScoped<IContaCorrenteService, ContaCorrenteService>();
        services.AddScoped<IEspecieService, EspecieService>();
        services.AddScoped<IPagamentoService, PagamentoService>();
        services.AddScoped<IRecebimentoService, RecebimentoService>();
        services.AddScoped<IHistoricoService, HistoricoService>();
        services.AddScoped<ILancamentoContabilService, LancamentoContabilService>();
        services.AddScoped<IUnidadeMedidaService, UnidadeMedidaService>();
        services.AddScoped<IMarcaService, MarcaService>();
        services.AddScoped<IGrupoService, GrupoService>();
        services.AddScoped<ILocalService, LocalService>();
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<IPrecoService, PrecoService>();
        services.AddScoped<IFechaEstoqueService, FechaEstoqueService>();
        services.AddScoped<IMovItensService, MovItensService>();
        services.AddScoped<IGrupoBemService, GrupoBemService>();
        services.AddScoped<IEstadoConservacaoService, EstadoConservacaoService>();
        services.AddScoped<IMotivoBaixaService, MotivoBaixaService>();
        services.AddScoped<IBaixaBemService, BaixaBemService>();
        services.AddScoped<IDepreciacaoService, DepreciacaoService>();
        services.AddScoped<IDespesaInvestimentoService, DespesaInvestimentoService>();
        services.AddScoped<IRelatorioService, RelatorioService>();
        services.AddScoped<IFiltroRelatorioService, FiltroRelatorioService>();
        services.AddScoped<ISolicitacaoRelatorioService, SolicitacaoRelatorioService>();
        services.AddScoped<IAuditoriaService, AuditoriaService>();
        services.AddScoped<ISaldoService, SaldoService>();

        return services;
    }
}
