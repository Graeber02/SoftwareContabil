using System;

namespace SoftwareContabil.Domain.Entities;

public class Marca
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cliforid { get; set; } = string.Empty;
    public CliFor? CliforidRef { get; set; }
}
