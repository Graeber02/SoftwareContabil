using System;

namespace SoftwareContabil.Domain.Entities;

public class Preco
{
    public int Id { get; set; }
    public decimal Valor { get; set; }
    public DateTime Datavigente { get; set; }
    public int? Produtoid { get; set; }
    public Produto? ProdutoidRef { get; set; }
}
