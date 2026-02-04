using TransactionService.Domain.Enums;
using TransactionService.Domain.Exceptions;

namespace TransactionService.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }

    public Guid CardId { get; private set; }

    public Guid FromAccountId {  get; private set; }

    public Guid ToAccountId { get; private set; }

    public decimal Amount { get; private set; }
    public string Currency {  get; private set; }

    public TransactionStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private Transaction() { }


    public Transaction (Guid BankCardId, Guid fromAccountId, Guid toAccountId, decimal amount, string currency)
    {
        if (fromAccountId == toAccountId)
            throw new DomainException("Accounts must be different");

        if (amount <= 0)
            throw new DomainException("Amount must be positive");

        Id = Guid.NewGuid();
        CardId = BankCardId;
        FromAccountId = fromAccountId;
        ToAccountId = toAccountId;
        Amount = amount;
        Currency = currency;
        Status = TransactionStatus.Created;
        CreatedAt = DateTime.UtcNow;

    }

    public void StartProcessing()
    {
        if (Status != TransactionStatus.Created)
            throw new DomainException("Only created transaction can be processed");

        Status = TransactionStatus.Processing;
    }

    public void Complete()
    {
        if (Status != TransactionStatus.Processing)
            throw new DomainException("Only processing transaction can be completed");

        Status = TransactionStatus.Completed;
    }

    public void Fail()
    {
        if (Status == TransactionStatus.Completed)
            throw new DomainException("Completed transaction cannot be failed");

        Status = TransactionStatus.Failed;
    }

}
