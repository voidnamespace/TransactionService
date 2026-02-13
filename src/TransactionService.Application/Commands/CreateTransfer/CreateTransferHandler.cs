using MediatR;
using TransactionService.Domain.Entities;
using TransactionService.Domain.ValueObjects;
using TransactionService.Application.Interfaces;
using TransactionService.Infrastructure.Messaging;
namespace TransactionService.Application.Commands.CreateTransfer;

public class CreateTransferHandler
    : IRequestHandler<CreateTransferCommand, Guid>
{
    private readonly ITransactionRepository _repository;
    private readonly IEventBus _eventBus;

    public CreateTransferHandler(
        ITransactionRepository repository,
        IEventBus eventBus)
    {
        _repository = repository;
        _eventBus = eventBus;
    }

    public async Task<Guid> Handle(
        CreateTransferCommand cmd,
        CancellationToken ct)
    {
        var money = new MoneyVO(cmd.Amount, cmd.Currency);

        var transaction = new Transaction(
            cardId: Guid.Empty, // пока ок
            fromAccountId: cmd.FromAccountId,
            toAccountId: cmd.ToAccountId,
            money: money);

        await _repository.AddAsync(transaction, ct);
        await _repository.SaveChangesAsync(ct);

        await _eventBus.PublishAsync(
            new TransferRequestedEvent(
                transaction.Id,
                transaction.FromAccountId,
                transaction.ToAccountId,
                transaction.Money.Amount,
                transaction.Money.Currency));

        return transaction.Id;
    }
}
