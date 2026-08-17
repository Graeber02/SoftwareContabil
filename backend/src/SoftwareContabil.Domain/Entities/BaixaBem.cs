using System;

namespace SoftwareContabil.Domain.Entities;

public class BaixaBem
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public double Valor { get; set; }
    public string? Observacao { get; set; }
    public int Motivobaixaid { get; set; }
    public MotivoBaixa? MotivobaixaidRef { get; set; }
    public int Patrimonioid { get; set; }
    public Patrimonio? PatrimonioidRef { get; set; }
    public string Cliforid { get; set; } = string.Empty;
    public CliFor? CliforidRef { get; set; }
}
