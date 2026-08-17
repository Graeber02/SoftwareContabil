using System;

namespace SoftwareContabil.Domain.Entities;

public class DespesaInvestimento
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public string? Observacao { get; set; }
    public double Valor { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public int Patrimonioid { get; set; }
    public Patrimonio? PatrimonioidRef { get; set; }
    public string Cliforid { get; set; } = string.Empty;
    public CliFor? CliforidRef { get; set; }
}
