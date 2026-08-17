namespace SoftwareContabil.Domain.Exceptions;

/// <summary>
/// Lançada quando uma regra de negócio é violada (ex.: pagar mais do que o
/// saldo do título, dar baixa em um bem já baixado). Mapeada para HTTP 400.
/// </summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
}
