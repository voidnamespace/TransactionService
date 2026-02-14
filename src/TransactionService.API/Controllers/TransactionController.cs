using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.Commands.CreateTransfer;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Enums;

namespace TransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer(
    [FromBody] CreateTransferRequest request,
    CancellationToken ct)
    {
        if (!Enum.TryParse<Currency>(
                request.Currency,
                ignoreCase: true,
                out var currency))
        {
            return BadRequest("Invalid currency");
        }

        var command = new CreateTransferCommand(
            request.FromAccountId,
            request.ToAccountId,
            request.Amount,
            currency);

        var transactionId = await _mediator.Send(command, ct);

        return Accepted(new { transactionId });
    }

}
