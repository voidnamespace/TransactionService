using TransactionService.Domain.Exceptions;

namespace TransactionService.Domain.ValueObjects;

public sealed class MoneyVO : IEquatable<MoneyVO>
{
    public decimal Amount { get; }
    public string Currency { get; } = string.Empty;

    private MoneyVO() { }
    public MoneyVO(decimal amount, string currency)
    {
        if (amount <= 0)
            throw new DomainException("Amount must be greater than zero");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency is required");

        if (currency.Length != 3)
            throw new DomainException("Currency must be ISO 4217 code (e.g. USD)");

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public bool Equals(MoneyVO? other)
    {
        if (other is null) return false;
        return Amount == other.Amount && Currency == other.Currency;
    }

    public override bool Equals(object? obj)
        => Equals(obj as MoneyVO);

    public override int GetHashCode()
        => HashCode.Combine(Amount, Currency);

    public override string ToString()
        => $"{Amount} {Currency}";
}
