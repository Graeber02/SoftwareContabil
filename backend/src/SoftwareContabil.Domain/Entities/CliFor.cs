using System.ComponentModel.DataAnnotations;

namespace SoftwareContabil.Domain.Entities;

public class CliFor
{
    [Required, StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Cnpj { get; set; }

    [StringLength(20)]
    public string? Cpf { get; set; }

    public char Tipopessoa { get; set; }

    [Required(AllowEmptyStrings = false), StringLength(255)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(255)]
    public string? Nomefantasia { get; set; }

    [EmailAddress, StringLength(255)]
    public string? Email { get; set; }

    [StringLength(30)]
    public string? Telefone { get; set; }

    [StringLength(30)]
    public string? Celular { get; set; }

    [StringLength(50)]
    public string? Tipocliente { get; set; }

    public int? Enderecoid { get; set; }
    public Endereco? EnderecoidRef { get; set; }
}
