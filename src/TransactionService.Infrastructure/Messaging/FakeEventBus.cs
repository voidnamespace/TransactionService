using TransactionService.Application.Interfaces;

namespace TransactionService.Infrastructure.Messaging;

public class FakeEventBus : IEventBus
{
    public Task PublishAsync<T>(T @event)
    {
        return Task.CompletedTask;
    }
}
