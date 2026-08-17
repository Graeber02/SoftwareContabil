using System;

namespace SoftwareContabil.Domain.Entities;

public class FechaEstoque
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public decimal Customedio { get; set; }
    public int Qtde { get; set; }
    public int Idproduto { get; set; }
    public Produto? IdprodutoRef { get; set; }
}
