using System;

namespace SoftwareContabil.Domain.Entities;

public class GrupoBem
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public double Taxadepreciacao { get; set; }
    public double Vidautil { get; set; }
    public string Cliforid { get; set; } = string.Empty;
    public CliFor? CliforidRef { get; set; }
}
