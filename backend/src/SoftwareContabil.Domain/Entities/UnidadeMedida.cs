using System;

namespace SoftwareContabil.Domain.Entities;

public class UnidadeMedida
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Sigla { get; set; } = string.Empty;
    public string Cliforid { get; set; } = string.Empty;
    public CliFor? CliforidRef { get; set; }
}
