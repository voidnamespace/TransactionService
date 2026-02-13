using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.Commands.CreateTransfer;

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
        [FromBody] CreateTransferCommand command,
        CancellationToken ct)
    {
        var transactionId = await _mediator.Send(command, ct);

        return Accepted(new
        {
            transactionId
        });
    }
}
