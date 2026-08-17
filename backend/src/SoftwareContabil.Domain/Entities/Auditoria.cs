
// Entidade de domínio gerada a partir do modelo original (migração Java -> .NET 10)

using System;

namespace SoftwareContabil.Domain.Entities;

public class Auditoria
{
    public int Id { get; set; }
    public string Tabela { get; set; } = string.Empty;
    public string? Valorantigo { get; set; }
    public string? Valornovo { get; set; }
    public string? Cliforid { get; set; }
}
