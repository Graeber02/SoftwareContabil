using System;

namespace SoftwareContabil.Domain.Entities;

public class Cidade
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public int Estadoid { get; set; }
    public Estado? EstadoidRef { get; set; }
}
