using System;

namespace SoftwareContabil.Domain.Entities;

public class LancamentoContabil
{
    public int Id { get; set; }
    public double Valor { get; set; }
    public DateTime Datahora { get; set; }
    public string? Historico { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public int Idconta { get; set; }
    public Conta? IdcontaRef { get; set; }
    public int? Centrocustoid { get; set; }
    public CentroCusto? CentrocustoidRef { get; set; }
    public string Cliforid { get; set; } = string.Empty;
    public CliFor? CliforidRef { get; set; }
}
