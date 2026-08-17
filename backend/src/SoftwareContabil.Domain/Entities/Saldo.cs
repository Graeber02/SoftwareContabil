using System;

namespace SoftwareContabil.Domain.Entities;

public class Saldo
{
    public int Id { get; set; }
    public double Qtde { get; set; }
    public double Valor { get; set; }
    public int Produtoid { get; set; }
    public Produto? ProdutoidRef { get; set; }
    public int Localid { get; set; }
    public Local? LocalidRef { get; set; }
}
