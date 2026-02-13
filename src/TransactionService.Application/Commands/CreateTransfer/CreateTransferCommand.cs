using MediatR;
namespace TransactionService.Application.Commands.CreateTransfer;

public record CreateTransferCommand(
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount,
    string Currency
) : IRequest<Guid>;
