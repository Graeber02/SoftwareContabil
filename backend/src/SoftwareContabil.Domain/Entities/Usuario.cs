
// Entidade de domínio gerada a partir do modelo original (migração Java -> .NET 10)

using System;

namespace SoftwareContabil.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}
