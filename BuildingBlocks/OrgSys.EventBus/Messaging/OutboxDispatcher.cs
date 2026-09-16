namespace OrgSys.Messaging;

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrgSys.SharedKernel;
using System.Text.Json;

public sealed class OutboxDispatcher(
    IServiceScopeFactory scopeFactory,
    ILogger<OutboxDispatcher> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await DispatchOnceAsync(stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogError(ex, "Outbox dispatch failed.");
            }

            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
        }
    }

    internal async Task DispatchOnceAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<OutboxMessage>>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

        var pending = (await repository.GetListByFilterAsync(m => m.ProcessedOn == null) ?? [])
            .OrderBy(m => m.OccurredOn)
            .Take(20)
            .ToList();

        foreach (var message in pending)
        {
            try
            {
                var type = Type.GetType(message.EventType, throwOnError: false);
                if (type is null)
                    throw new InvalidOperationException($"Unknown outbox type '{message.EventType}'.");

                var evt = (IIntegrationEvent?)JsonSerializer.Deserialize(message.Payload, type);
                if (evt is null)
                    throw new InvalidOperationException("Outbox payload deserialized to null.");

                await publisher.Publish(evt, cancellationToken);
                message.ProcessedOn = DateTime.UtcNow;
                message.Error = null;
            }
            catch (Exception ex)
            {
                message.Error = ex.Message.Length > 1000 ? ex.Message[..1000] : ex.Message;
                logger.LogError(ex, "Failed to dispatch outbox {EventId}", message.EventId);
            }

            await repository.UpdateAsync(message);
        }

        if (pending.Count > 0)
            await unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
