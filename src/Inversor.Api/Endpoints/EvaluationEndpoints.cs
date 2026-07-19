using Inversor.Core.Application.DTOs.Request;
using Inversor.Core.Application.UseCases;

namespace Inversor.Api.Endpoints;

public static class EvaluationEndpoints
{
    public static void MapEvaluationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/evaluations").WithTags("Evaluations");

        group.MapPost("/", async (
            EvaluateTextRequest request,
            EvaluateTranslationUseCase useCase,
            CancellationToken ct) =>
        {
            var result = await useCase.ExecuteAsync(request, ct);
            return Results.Ok(result);
        })
        .WithName("EvaluateTranslationText")
        .WithSummary("Evaluates a text, returns grammatical feedback, and updates the SRS engine.");
    }
}
