using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;
using SoftwareContabil.Infrastructure.Repositories;
using SoftwareContabil.Infrastructure.Security;

namespace SoftwareContabil.Infrastructure;

/// <summary>
/// Ponto único de registro da camada de Infraestrutura: banco de dados,
/// repositórios (um por entidade) e
/// serviços técnicos (hash de senha, geração de token).
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));
        services.AddSingleton<DapperContext>();

        // Repositórios — um por entidade, cada um com sua própria interface e implementação.
        services.AddScoped<IContaPagarRepository, ContaPagarRepository>();
        services.AddScoped<IContaReceberRepository, ContaReceberRepository>();
        services.AddScoped<IPatrimonioRepository, PatrimonioRepository>();
        services.AddScoped<IContaRepository, ContaRepository>();
        services.AddScoped<IMovimentacaoRepository, MovimentacaoRepository>();
        services.AddScoped<ISaldoRepository, SaldoRepository>();
        services.AddScoped<IEstadoRepository, EstadoRepository>();
        services.AddScoped<ICidadeRepository, CidadeRepository>();
        services.AddScoped<IEnderecoRepository, EnderecoRepository>();
        services.AddScoped<ICliForRepository, CliForRepository>();
        services.AddScoped<ICentroCustoRepository, CentroCustoRepository>();
        services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
        services.AddScoped<IEspecieRepository, EspecieRepository>();
        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        services.AddScoped<IRecebimentoRepository, RecebimentoRepository>();
        services.AddScoped<IHistoricoRepository, HistoricoRepository>();
        services.AddScoped<ILancamentoContabilRepository, LancamentoContabilRepository>();
        services.AddScoped<IUnidadeMedidaRepository, UnidadeMedidaRepository>();
        services.AddScoped<IMarcaRepository, MarcaRepository>();
        services.AddScoped<IGrupoRepository, GrupoRepository>();
        services.AddScoped<ILocalRepository, LocalRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IPrecoRepository, PrecoRepository>();
        services.AddScoped<IFechaEstoqueRepository, FechaEstoqueRepository>();
        services.AddScoped<IMovItensRepository, MovItensRepository>();
        services.AddScoped<IGrupoBemRepository, GrupoBemRepository>();
        services.AddScoped<IEstadoConservacaoRepository, EstadoConservacaoRepository>();
        services.AddScoped<IMotivoBaixaRepository, MotivoBaixaRepository>();
        services.AddScoped<IBaixaBemRepository, BaixaBemRepository>();
        services.AddScoped<IDepreciacaoRepository, DepreciacaoRepository>();
        services.AddScoped<IDespesaInvestimentoRepository, DespesaInvestimentoRepository>();
        services.AddScoped<IRelatorioRepository, RelatorioRepository>();
        services.AddScoped<IFiltroRelatorioRepository, FiltroRelatorioRepository>();
        services.AddScoped<ISolicitacaoRelatorioRepository, SolicitacaoRelatorioRepository>();
        services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IGerencialRepository, GerencialRepository>();

        // Serviços técnicos usados pela Application via abstração.
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddScoped<ITokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
