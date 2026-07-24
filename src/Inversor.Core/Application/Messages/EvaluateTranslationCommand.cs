using Inversor.Core.Domain.Enums;

namespace Inversor.Core.Application.Messages;
/// <summary>
/// Command issued by the API and queued in RabbitMQ for the Worker to execute the evaluation with Gemini.
/// </summary>
public record EvaluateTranslationCommand(
    Guid SubmissionId,
    Guid UserLanguageProfileId,
    SubmissionMode Mode,
    string Text
);