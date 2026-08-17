using System.ComponentModel.DataAnnotations;

namespace SoftwareContabil.Domain.Entities;

public class ContaPagar
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false), StringLength(255)]
    public string Descricao { get; set; } = string.Empty;

    public double Numdocumento { get; set; }
    public double Seriedocumento { get; set; }
    public DateTime Datadocumento { get; set; }
    public DateTime Datavencimento { get; set; }
    public DateTime? Datapagamento { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero.")]
    public double Valor { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Saldo não pode ser negativo.")]
    public double Saldo { get; set; }

    [Required, StringLength(20)]
    public string Cliforid { get; set; } = string.Empty;
    public CliFor? CliforidRef { get; set; }

    [Required, StringLength(20)]
    public string Fornecedorid { get; set; } = string.Empty;
    public CliFor? FornecedoridRef { get; set; }
}
