namespace SoftwareContabil.Domain.Exceptions;

/// <summary>Lançada quando uma entidade solicitada não existe. Mapeada para HTTP 404.</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string entityName, object key)
        : base($"{entityName} com id '{key}' não foi encontrado(a).") { }
}
