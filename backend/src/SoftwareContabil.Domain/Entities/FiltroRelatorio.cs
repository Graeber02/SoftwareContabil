using System;

namespace SoftwareContabil.Domain.Entities;

public class FiltroRelatorio
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Excluido { get; set; }
    public string? Sqlwhere { get; set; }
    public int Relatorioid { get; set; }
    public Relatorio? RelatorioidRef { get; set; }
}
