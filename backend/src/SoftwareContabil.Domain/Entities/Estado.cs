
// Entidade de domínio gerada a partir do modelo original (migração Java -> .NET 10)

using System;

namespace SoftwareContabil.Domain.Entities;

public class Estado
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Sigla { get; set; } = string.Empty;
}
