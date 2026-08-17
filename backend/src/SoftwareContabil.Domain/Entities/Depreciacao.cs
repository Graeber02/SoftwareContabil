
// Entidade de domínio gerada a partir do modelo original (migração Java -> .NET 10)

using System;

namespace SoftwareContabil.Domain.Entities;

public class Depreciacao
{
    public int Id { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
    public double Valordepreciado { get; set; }
    public double Valorreavaliado { get; set; }
    public double Vidautil { get; set; }
    public double Taxadepreciacaomensal { get; set; }
    public double Taxadepreciacaoanual { get; set; }
    public double Valoratualizado { get; set; }
    public int? Deprecia { get; set; }
    public DateTime Datadepreciacao { get; set; }
    public double Valoranual { get; set; }
    public double Valormes { get; set; }
    public int Patrimonioid { get; set; }
    public Patrimonio? PatrimonioidRef { get; set; }
    public string Cliforid { get; set; } = string.Empty;
    public CliFor? CliforidRef { get; set; }
}
