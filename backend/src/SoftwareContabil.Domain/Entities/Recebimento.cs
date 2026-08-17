using System;

namespace SoftwareContabil.Domain.Entities;

public class Recebimento
{
    public int Id { get; set; }
    public double Valorrecebido { get; set; }
    public DateTime Datarecebimento { get; set; }
    public string? Descricao { get; set; }
    public int Contareceberid { get; set; }
    public ContaReceber? ContareceberidRef { get; set; }
    public int Especieid { get; set; }
    public Especie? EspecieidRef { get; set; }
    public int Contacorrenteid { get; set; }
    public ContaCorrente? ContacorrenteidRef { get; set; }
    public string Cliforid { get; set; } = string.Empty;
    public CliFor? CliforidRef { get; set; }
}
