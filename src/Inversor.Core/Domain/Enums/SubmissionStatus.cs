
namespace Inversor.Core.Domain.Enums;

public enum SubmissionStatus
{
    Pending,     // 1. Requested received, waiting in the queue (RabbitMQ).
    Processing,  // 2. The Worker took it and is calling Gemini.
    Completed,   // 3. Total success. We have the tags.
    Failed       // 4. Polly ran out of retries (e.g., Gemini down).
}
