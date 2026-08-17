
using System.ComponentModel.DataAnnotations;

namespace SoftwareContabil.Domain.Entities;

public class Patrimonio
{
    public int Id { get; set; }
    public DateTime Dataaquisicao { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Valor não pode ser negativo.")]
    public double Valor { get; set; }

    [StringLength(500)]
    public string? Observacao { get; set; }

    public int Baixado { get; set; }
    public bool Depreciavel { get; set; }

    public int Centrocustoid { get; set; }
    public CentroCusto? CentrocustoidRef { get; set; }

    [Required, StringLength(20)]
    public string Fornecedorid { get; set; } = string.Empty;
    public CliFor? FornecedoridRef { get; set; }

    [Required, StringLength(20)]
    public string Cliforid { get; set; } = string.Empty;
    public CliFor? CliforidRef { get; set; }

    public int Estadoconservacaoid { get; set; }
    public EstadoConservacao? EstadoconservacaoidRef { get; set; }

    public int Grupobemid { get; set; }
    public GrupoBem? GrupobemidRef { get; set; }

    public int? Produtoid { get; set; }
    public Produto? ProdutoidRef { get; set; }
}
