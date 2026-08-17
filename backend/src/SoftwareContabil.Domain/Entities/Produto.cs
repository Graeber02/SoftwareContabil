using System.ComponentModel.DataAnnotations;

namespace SoftwareContabil.Domain.Entities;

public class Produto
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false), StringLength(255)]
    public string Nome { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false), StringLength(255)]
    public string Descricao { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Quantidade mínima não pode ser negativa.")]
    public double Quantidademinima { get; set; }

    [Required, StringLength(20)]
    public string Cliforid { get; set; } = string.Empty;
    public CliFor? CliforidRef { get; set; }

    public int Grupoid { get; set; }
    public Grupo? GrupoidRef { get; set; }

    public int Marcaid { get; set; }
    public Marca? MarcaidRef { get; set; }

    public int Unidademedidaid { get; set; }
    public UnidadeMedida? UnidademedidaidRef { get; set; }

    [Required, StringLength(20)]
    public string Fornecedorid { get; set; } = string.Empty;
    public CliFor? FornecedoridRef { get; set; }
}
