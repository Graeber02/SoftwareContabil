using System;

namespace SoftwareContabil.Domain.Entities;

public class Movimentacao
{
    public int Id { get; set; }
    public string? Notafiscal { get; set; }
    public char Tipo { get; set; }
    public DateTime Data { get; set; }
    public double Valortotal { get; set; }
    public string Cliforid { get; set; } = string.Empty;
    public CliFor? CliforidRef { get; set; }
    public string Empresaid { get; set; } = string.Empty;
    public CliFor? EmpresaidRef { get; set; }
}
