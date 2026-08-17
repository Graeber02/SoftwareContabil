using System;
using System.ComponentModel.DataAnnotations;

namespace SoftwareContabil.Domain.Entities;

public class Conta
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false), StringLength(255)]
    public string Descricao { get; set; } = string.Empty;

    public bool Sintetica { get; set; }
    public int Ordem { get; set; }
    public string? Hierarquia { get; set; }

    [Required, StringLength(20)]
    public string Cliforid { get; set; } = string.Empty;
    public CliFor? CliforidRef { get; set; }

    public int? Contapai { get; set; }
    public Conta? ContapaiRef { get; set; }
}
