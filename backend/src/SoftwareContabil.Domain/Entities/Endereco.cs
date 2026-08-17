using System;

namespace SoftwareContabil.Domain.Entities;

public class Endereco
{
    public int Id { get; set; }
    public string Cep { get; set; } = string.Empty;
    public string Rua { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public int Numero { get; set; }
    public string? Complemento { get; set; }
    public int Cidadeid { get; set; }
    public Cidade? CidadeidRef { get; set; }
}
