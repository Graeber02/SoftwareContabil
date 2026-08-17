using System;

namespace SoftwareContabil.Domain.Entities;

public class Pagamento
{
    public int Id { get; set; }
    public double Valorpago { get; set; }
    public DateTime Datapagamento { get; set; }
    public string? Descricao { get; set; }
    public int Contapagarid { get; set; }
    public ContaPagar? ContapagaridRef { get; set; }
    public int Especieid { get; set; }
    public Especie? EspecieidRef { get; set; }
    public int Contacorrenteid { get; set; }
    public ContaCorrente? ContacorrenteidRef { get; set; }
    public string Cliforid { get; set; } = string.Empty;
    public CliFor? CliforidRef { get; set; }
}
