
// DbContext gerado a partir do modelo original (migração Java -> .NET 10)

using Microsoft.EntityFrameworkCore;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Estado> Estados => Set<Estado>();
    public DbSet<Cidade> Cidades => Set<Cidade>();
    public DbSet<Endereco> Enderecos => Set<Endereco>();
    public DbSet<CliFor> CliFors => Set<CliFor>();
    public DbSet<CentroCusto> CentroCustos => Set<CentroCusto>();
    public DbSet<Conta> Contas => Set<Conta>();
    public DbSet<ContaCorrente> ContaCorrentes => Set<ContaCorrente>();
    public DbSet<ContaPagar> ContaPagars => Set<ContaPagar>();
    public DbSet<ContaReceber> ContaRecebers => Set<ContaReceber>();
    public DbSet<Especie> Especies => Set<Especie>();
    public DbSet<Pagamento> Pagamentos => Set<Pagamento>();
    public DbSet<Recebimento> Recebimentos => Set<Recebimento>();
    public DbSet<Historico> Historicos => Set<Historico>();
    public DbSet<LancamentoContabil> LancamentoContabils => Set<LancamentoContabil>();
    public DbSet<UnidadeMedida> UnidadeMedidas => Set<UnidadeMedida>();
    public DbSet<Marca> Marcas => Set<Marca>();
    public DbSet<Grupo> Grupos => Set<Grupo>();
    public DbSet<Local> Locals => Set<Local>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Preco> Precos => Set<Preco>();
    public DbSet<Saldo> Saldos => Set<Saldo>();
    public DbSet<FechaEstoque> FechaEstoques => Set<FechaEstoque>();
    public DbSet<Movimentacao> Movimentacaos => Set<Movimentacao>();
    public DbSet<MovItens> MovItensList => Set<MovItens>();
    public DbSet<GrupoBem> GrupoBems => Set<GrupoBem>();
    public DbSet<EstadoConservacao> EstadoConservacaos => Set<EstadoConservacao>();
    public DbSet<MotivoBaixa> MotivoBaixas => Set<MotivoBaixa>();
    public DbSet<Patrimonio> Patrimonios => Set<Patrimonio>();
    public DbSet<BaixaBem> BaixaBems => Set<BaixaBem>();
    public DbSet<Depreciacao> Depreciacaos => Set<Depreciacao>();
    public DbSet<DespesaInvestimento> DespesaInvestimentos => Set<DespesaInvestimento>();
    public DbSet<Relatorio> Relatorios => Set<Relatorio>();
    public DbSet<FiltroRelatorio> FiltroRelatorios => Set<FiltroRelatorio>();
    public DbSet<SolicitacaoRelatorio> SolicitacaoRelatorios => Set<SolicitacaoRelatorio>();
    public DbSet<Auditoria> Auditorias => Set<Auditoria>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Estado>(e =>
        {
            e.ToTable("estado");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Descricao).HasColumnName("descricao");
            e.Property(x => x.Sigla).HasColumnName("sigla");
        });

        modelBuilder.Entity<Cidade>(e =>
        {
            e.ToTable("cidade");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Descricao).HasColumnName("descricao");
            e.Property(x => x.Estadoid).HasColumnName("estadoid");
            e.HasOne(x => x.EstadoidRef).WithMany().HasForeignKey(x => x.Estadoid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Endereco>(e =>
        {
            e.ToTable("endereco");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Cep).HasColumnName("cep");
            e.Property(x => x.Rua).HasColumnName("rua");
            e.Property(x => x.Bairro).HasColumnName("bairro");
            e.Property(x => x.Numero).HasColumnName("numero");
            e.Property(x => x.Complemento).HasColumnName("complemento");
            e.Property(x => x.Cidadeid).HasColumnName("cidadeid");
            e.HasOne(x => x.CidadeidRef).WithMany().HasForeignKey(x => x.Cidadeid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CliFor>(e =>
        {
            e.ToTable("clifor");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").HasMaxLength(20);
            e.Property(x => x.Cnpj).HasColumnName("cnpj");
            e.Property(x => x.Cpf).HasColumnName("cpf");
            e.Property(x => x.Tipopessoa).HasColumnName("tipopessoa");
            e.Property(x => x.Nome).HasColumnName("nome");
            e.Property(x => x.Nomefantasia).HasColumnName("nomefantasia");
            e.Property(x => x.Email).HasColumnName("email");
            e.Property(x => x.Telefone).HasColumnName("telefone");
            e.Property(x => x.Celular).HasColumnName("celular");
            e.Property(x => x.Tipocliente).HasColumnName("tipocliente");
            e.Property(x => x.Enderecoid).HasColumnName("enderecoid");
            e.HasOne(x => x.EnderecoidRef).WithMany().HasForeignKey(x => x.Enderecoid).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Especie>(e =>
        {
            e.ToTable("especie");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Descricao).HasColumnName("descricao");
        });

        modelBuilder.Entity<Historico>(e =>
        {
            e.ToTable("historico");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Descricao).HasColumnName("descricao");
            e.Property(x => x.Conteudo).HasColumnName("conteudo");
        });

        modelBuilder.Entity<UnidadeMedida>(e =>
        {
            e.ToTable("unidademedida");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Descricao).HasColumnName("descricao");
            e.Property(x => x.Sigla).HasColumnName("sigla");
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Marca>(e =>
        {
            e.ToTable("marca");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Nome).HasColumnName("nome");
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Grupo>(e =>
        {
            e.ToTable("grupo");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Descricao).HasColumnName("descricao");
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Local>(e =>
        {
            e.ToTable("local");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Descricao).HasColumnName("descricao");
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<GrupoBem>(e =>
        {
            e.ToTable("grupobem");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Descricao).HasColumnName("descricao");
            e.Property(x => x.Taxadepreciacao).HasColumnName("taxadepreciacao");
            e.Property(x => x.Vidautil).HasColumnName("vidautil");
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<EstadoConservacao>(e =>
        {
            e.ToTable("estadoconservacao");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Descricao).HasColumnName("descricao");
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<MotivoBaixa>(e =>
        {
            e.ToTable("motivobaixa");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Descricao).HasColumnName("descricao");
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CentroCusto>(e =>
        {
            e.ToTable("centrocusto");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Nome).HasColumnName("nome");
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Conta>(e =>
        {
            e.ToTable("conta");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Descricao).HasColumnName("descricao");
            e.Property(x => x.Sintetica).HasColumnName("sintetica");
            e.Property(x => x.Ordem).HasColumnName("ordem");
            e.Property(x => x.Hierarquia).HasColumnName("hierarquia");
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Contapai).HasColumnName("contapai");
            e.HasOne(x => x.ContapaiRef).WithMany().HasForeignKey(x => x.Contapai).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ContaCorrente>(e =>
        {
            e.ToTable("contacorrente");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Descricao).HasColumnName("descricao");
            e.Property(x => x.Agencia).HasColumnName("agencia");
            e.Property(x => x.Conta).HasColumnName("conta");
            e.Property(x => x.Codbanco).HasColumnName("codbanco");
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ContaPagar>(e =>
        {
            e.ToTable("contapagar");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Descricao).HasColumnName("descricao");
            e.Property(x => x.Numdocumento).HasColumnName("numdocumento");
            e.Property(x => x.Seriedocumento).HasColumnName("seriedocumento");
            e.Property(x => x.Datadocumento).HasColumnName("datadocumento");
            e.Property(x => x.Datavencimento).HasColumnName("datavencimento");
            e.Property(x => x.Datapagamento).HasColumnName("datapagamento");
            e.Property(x => x.Valor).HasColumnName("valor");
            e.Property(x => x.Saldo).HasColumnName("saldo");
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Fornecedorid).HasColumnName("fornecedorid");
            e.HasOne(x => x.FornecedoridRef).WithMany().HasForeignKey(x => x.Fornecedorid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ContaReceber>(e =>
        {
            e.ToTable("contareceber");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Descricao).HasColumnName("descricao");
            e.Property(x => x.Numdocumento).HasColumnName("numdocumento");
            e.Property(x => x.Datadocumento).HasColumnName("datadocumento");
            e.Property(x => x.Datavencimento).HasColumnName("datavencimento");
            e.Property(x => x.Datarecebimento).HasColumnName("datarecebimento");
            e.Property(x => x.Valor).HasColumnName("valor");
            e.Property(x => x.Saldo).HasColumnName("saldo");
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Pagamento>(e =>
        {
            e.ToTable("pagamento");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Valorpago).HasColumnName("valorpago");
            e.Property(x => x.Datapagamento).HasColumnName("datapagamento");
            e.Property(x => x.Descricao).HasColumnName("descricao");
            e.Property(x => x.Contapagarid).HasColumnName("contapagarid");
            e.HasOne(x => x.ContapagaridRef).WithMany().HasForeignKey(x => x.Contapagarid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Especieid).HasColumnName("especieid");
            e.HasOne(x => x.EspecieidRef).WithMany().HasForeignKey(x => x.Especieid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Contacorrenteid).HasColumnName("contacorrenteid");
            e.HasOne(x => x.ContacorrenteidRef).WithMany().HasForeignKey(x => x.Contacorrenteid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Recebimento>(e =>
        {
            e.ToTable("recebimento");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Valorrecebido).HasColumnName("valorrecebido");
            e.Property(x => x.Datarecebimento).HasColumnName("datarecebimento");
            e.Property(x => x.Descricao).HasColumnName("descricao");
            e.Property(x => x.Contareceberid).HasColumnName("contareceberid");
            e.HasOne(x => x.ContareceberidRef).WithMany().HasForeignKey(x => x.Contareceberid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Especieid).HasColumnName("especieid");
            e.HasOne(x => x.EspecieidRef).WithMany().HasForeignKey(x => x.Especieid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Contacorrenteid).HasColumnName("contacorrenteid");
            e.HasOne(x => x.ContacorrenteidRef).WithMany().HasForeignKey(x => x.Contacorrenteid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<LancamentoContabil>(e =>
        {
            e.ToTable("lancamentocontabil");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Valor).HasColumnName("valor");
            e.Property(x => x.Datahora).HasColumnName("datahora");
            e.Property(x => x.Historico).HasColumnName("historico");
            e.Property(x => x.Tipo).HasColumnName("tipo");
            e.Property(x => x.Idconta).HasColumnName("idconta");
            e.HasOne(x => x.IdcontaRef).WithMany().HasForeignKey(x => x.Idconta).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Centrocustoid).HasColumnName("centrocustoid");
            e.HasOne(x => x.CentrocustoidRef).WithMany().HasForeignKey(x => x.Centrocustoid).OnDelete(DeleteBehavior.SetNull);
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Produto>(e =>
        {
            e.ToTable("produto");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Nome).HasColumnName("nome");
            e.Property(x => x.Descricao).HasColumnName("descricao");
            e.Property(x => x.Quantidademinima).HasColumnName("quantidademinima");
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Grupoid).HasColumnName("grupoid");
            e.HasOne(x => x.GrupoidRef).WithMany().HasForeignKey(x => x.Grupoid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Marcaid).HasColumnName("marcaid");
            e.HasOne(x => x.MarcaidRef).WithMany().HasForeignKey(x => x.Marcaid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Unidademedidaid).HasColumnName("unidademedidaid");
            e.HasOne(x => x.UnidademedidaidRef).WithMany().HasForeignKey(x => x.Unidademedidaid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Fornecedorid).HasColumnName("fornecedorid");
            e.HasOne(x => x.FornecedoridRef).WithMany().HasForeignKey(x => x.Fornecedorid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Preco>(e =>
        {
            e.ToTable("preco");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Valor).HasColumnName("valor");
            e.Property(x => x.Datavigente).HasColumnName("datavigente");
            e.Property(x => x.Produtoid).HasColumnName("produtoid");
            e.HasOne(x => x.ProdutoidRef).WithMany().HasForeignKey(x => x.Produtoid).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Saldo>(e =>
        {
            e.ToTable("saldo");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Qtde).HasColumnName("qtde");
            e.Property(x => x.Valor).HasColumnName("valor");
            e.Property(x => x.Produtoid).HasColumnName("produtoid");
            e.HasOne(x => x.ProdutoidRef).WithMany().HasForeignKey(x => x.Produtoid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Localid).HasColumnName("localid");
            e.HasOne(x => x.LocalidRef).WithMany().HasForeignKey(x => x.Localid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FechaEstoque>(e =>
        {
            e.ToTable("fechaestoque");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Data).HasColumnName("data");
            e.Property(x => x.Customedio).HasColumnName("customedio");
            e.Property(x => x.Qtde).HasColumnName("qtde");
            e.Property(x => x.Idproduto).HasColumnName("idproduto");
            e.HasOne(x => x.IdprodutoRef).WithMany().HasForeignKey(x => x.Idproduto).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Movimentacao>(e =>
        {
            e.ToTable("movimentacao");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Notafiscal).HasColumnName("notafiscal");
            e.Property(x => x.Tipo).HasColumnName("tipo");
            e.Property(x => x.Data).HasColumnName("data");
            e.Property(x => x.Valortotal).HasColumnName("valortotal");
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Empresaid).HasColumnName("empresaid");
            e.HasOne(x => x.EmpresaidRef).WithMany().HasForeignKey(x => x.Empresaid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<MovItens>(e =>
        {
            e.ToTable("movitens");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Sequencia).HasColumnName("sequencia");
            e.Property(x => x.Quantidade).HasColumnName("quantidade");
            e.Property(x => x.Valor).HasColumnName("valor");
            e.Property(x => x.Movimentacaoid).HasColumnName("movimentacaoid");
            e.HasOne(x => x.MovimentacaoidRef).WithMany().HasForeignKey(x => x.Movimentacaoid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Produtoid).HasColumnName("produtoid");
            e.HasOne(x => x.ProdutoidRef).WithMany().HasForeignKey(x => x.Produtoid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Localid).HasColumnName("localid");
            e.HasOne(x => x.LocalidRef).WithMany().HasForeignKey(x => x.Localid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Patrimonio>(e =>
        {
            e.ToTable("patrimonio");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Dataaquisicao).HasColumnName("dataaquisicao");
            e.Property(x => x.Valor).HasColumnName("valor");
            e.Property(x => x.Observacao).HasColumnName("observacao");
            e.Property(x => x.Baixado).HasColumnName("baixado");
            e.Property(x => x.Depreciavel).HasColumnName("depreciavel");
            e.Property(x => x.Centrocustoid).HasColumnName("centrocustoid");
            e.HasOne(x => x.CentrocustoidRef).WithMany().HasForeignKey(x => x.Centrocustoid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Fornecedorid).HasColumnName("fornecedorid");
            e.HasOne(x => x.FornecedoridRef).WithMany().HasForeignKey(x => x.Fornecedorid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Estadoconservacaoid).HasColumnName("estadoconservacaoid");
            e.HasOne(x => x.EstadoconservacaoidRef).WithMany().HasForeignKey(x => x.Estadoconservacaoid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Grupobemid).HasColumnName("grupobemid");
            e.HasOne(x => x.GrupobemidRef).WithMany().HasForeignKey(x => x.Grupobemid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Produtoid).HasColumnName("produtoid");
            e.HasOne(x => x.ProdutoidRef).WithMany().HasForeignKey(x => x.Produtoid).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<BaixaBem>(e =>
        {
            e.ToTable("baixabem");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Data).HasColumnName("data");
            e.Property(x => x.Valor).HasColumnName("valor");
            e.Property(x => x.Observacao).HasColumnName("observacao");
            e.Property(x => x.Motivobaixaid).HasColumnName("motivobaixaid");
            e.HasOne(x => x.MotivobaixaidRef).WithMany().HasForeignKey(x => x.Motivobaixaid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Patrimonioid).HasColumnName("patrimonioid");
            e.HasOne(x => x.PatrimonioidRef).WithMany().HasForeignKey(x => x.Patrimonioid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Depreciacao>(e =>
        {
            e.ToTable("depreciacao");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Mes).HasColumnName("mes");
            e.Property(x => x.Ano).HasColumnName("ano");
            e.Property(x => x.Valordepreciado).HasColumnName("valordepreciado");
            e.Property(x => x.Valorreavaliado).HasColumnName("valorreavaliado");
            e.Property(x => x.Vidautil).HasColumnName("vidautil");
            e.Property(x => x.Taxadepreciacaomensal).HasColumnName("taxadepreciacaomensal");
            e.Property(x => x.Taxadepreciacaoanual).HasColumnName("taxadepreciacaoanual");
            e.Property(x => x.Valoratualizado).HasColumnName("valoratualizado");
            e.Property(x => x.Deprecia).HasColumnName("deprecia");
            e.Property(x => x.Datadepreciacao).HasColumnName("datadepreciacao");
            e.Property(x => x.Valoranual).HasColumnName("valoranual");
            e.Property(x => x.Valormes).HasColumnName("valormes");
            e.Property(x => x.Patrimonioid).HasColumnName("patrimonioid");
            e.HasOne(x => x.PatrimonioidRef).WithMany().HasForeignKey(x => x.Patrimonioid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DespesaInvestimento>(e =>
        {
            e.ToTable("despesainvestimento");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Data).HasColumnName("data");
            e.Property(x => x.Observacao).HasColumnName("observacao");
            e.Property(x => x.Valor).HasColumnName("valor");
            e.Property(x => x.Tipo).HasColumnName("tipo");
            e.Property(x => x.Patrimonioid).HasColumnName("patrimonioid");
            e.HasOne(x => x.PatrimonioidRef).WithMany().HasForeignKey(x => x.Patrimonioid).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.HasOne(x => x.CliforidRef).WithMany().HasForeignKey(x => x.Cliforid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Relatorio>(e =>
        {
            e.ToTable("relatorio");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Nome).HasColumnName("nome");
            e.Property(x => x.Excluido).HasColumnName("excluido");
            e.Property(x => x.Sqlquery).HasColumnName("sqlquery");
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
        });

        modelBuilder.Entity<FiltroRelatorio>(e =>
        {
            e.ToTable("filtrorelatorio");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Nome).HasColumnName("nome");
            e.Property(x => x.Excluido).HasColumnName("excluido");
            e.Property(x => x.Sqlwhere).HasColumnName("sqlwhere");
            e.Property(x => x.Relatorioid).HasColumnName("relatorioid");
            e.HasOne(x => x.RelatorioidRef).WithMany().HasForeignKey(x => x.Relatorioid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SolicitacaoRelatorio>(e =>
        {
            e.ToTable("solicitacaorelatorio");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Descricao).HasColumnName("descricao");
            e.Property(x => x.Tipo).HasColumnName("tipo");
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
            e.Property(x => x.Relatorioid).HasColumnName("relatorioid");
            e.HasOne(x => x.RelatorioidRef).WithMany().HasForeignKey(x => x.Relatorioid).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Auditoria>(e =>
        {
            e.ToTable("auditoria");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Tabela).HasColumnName("tabela");
            e.Property(x => x.Valorantigo).HasColumnName("valorantigo");
            e.Property(x => x.Valornovo).HasColumnName("valornovo");
            e.Property(x => x.Cliforid).HasColumnName("cliforid");
        });

        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("usuario");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Email).HasColumnName("email");
            e.Property(x => x.Senha).HasColumnName("senha");
        });

    }
}
