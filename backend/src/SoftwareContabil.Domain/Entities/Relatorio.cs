using System;

namespace SoftwareContabil.Domain.Entities;

public class Relatorio
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Excluido { get; set; }
    public string? Sqlquery { get; set; }
    public string? Cliforid { get; set; }
}
