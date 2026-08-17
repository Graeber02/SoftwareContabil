using System;

namespace SoftwareContabil.Domain.Entities;

public class SolicitacaoRelatorio
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public int Tipo { get; set; }
    public string? Cliforid { get; set; }
    public int Relatorioid { get; set; }
    public Relatorio? RelatorioidRef { get; set; }
}
