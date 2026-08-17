using System;

namespace SoftwareContabil.Domain.Entities;

public class ContaCorrente
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public double Agencia { get; set; }
    public string Conta { get; set; } = string.Empty;
    public double Codbanco { get; set; }
    public string Cliforid { get; set; } = string.Empty;
    public CliFor? CliforidRef { get; set; }
}
