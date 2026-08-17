using System;

namespace SoftwareContabil.Domain.Entities;

public class MovItens
{
    public int Id { get; set; }
    public int Sequencia { get; set; }
    public double Quantidade { get; set; }
    public double Valor { get; set; }
    public int Movimentacaoid { get; set; }
    public Movimentacao? MovimentacaoidRef { get; set; }
    public int Produtoid { get; set; }
    public Produto? ProdutoidRef { get; set; }
    public int Localid { get; set; }
    public Local? LocalidRef { get; set; }
    public string Cliforid { get; set; } = string.Empty;
    public CliFor? CliforidRef { get; set; }
}
