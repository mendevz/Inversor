using Inversor.Api.Hubs;
using Inversor.Core.Application.Messages;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Inversor.Api.Consumers;

/// <summary>
/// Represents a MassTransit consumer that handles the EvaluationCompletedEvent 
/// and sends the evaluation result to connected clients via SignalR.
/// </summary>
/// <param name="hubContext"></param>
/// <param name="logger"></param>
public class EvaluationCompletedConsumer(
    IHubContext<NotificationHub> hubContext,
    ILogger<EvaluationCompletedConsumer> logger) : IConsumer<EvaluationCompletedEvent>
{
    public async Task Consume(ConsumeContext<EvaluationCompletedEvent> context)
    {
        var message = context.Message;
        logger.LogInformation("Received EvaluationCompletedEvent for Submission {SubmissionId} via RabbitMQ.", 
            message.SubmissionId);

        // if contains a SignalR connection ID, send the message to that specific client
        if (!string.IsNullOrWhiteSpace(message.SignalRConnectionId))
        {
            await hubContext.Clients.Client(message.SignalRConnectionId)
                .SendAsync("ReceiveEvaluationResult", message, context.CancellationToken);
        }
        else
        {
            await hubContext.Clients.All
                .SendAsync("ReceiveEvaluationResult", message, context.CancellationToken);
        }
    }
}
