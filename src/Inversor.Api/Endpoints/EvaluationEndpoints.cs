using Inversor.Core.Application.DTOs.Request;
using Inversor.Core.Application.UseCases;

namespace Inversor.Api.Endpoints;

public static class EvaluationEndpoints
{
    public static void MapEvaluationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/evaluations").WithTags("Evaluations");

        group.MapPost("/", async (
            EvaluateTranslationRequest request,
            EvaluateTranslationUseCase useCase,
            CancellationToken ct) =>
        {
            var result = await useCase.ExecuteAsync(request, ct);
            return Results.Accepted($"/api/evaluations/{result.SubmissionId}", result);
        })
        .WithName("EvaluateTranslationText")
        .WithSummary("Enqueues a text evaluation request asynchronously.");


        // GET /api/evaluations/{id} -> Fallback endpoint to check status or recover results
        group.MapGet("/{id:guid}", async (
            Guid id,
            GetTranslationSubmissionStatusUseCase useCase,
            CancellationToken ct) =>
        {
            var result = await useCase.ExecuteAsync(id, ct);
            return Results.Ok(result);
        })
        .WithName("GetEvaluationStatus")
        .WithSummary("Gets the current processing status and results of a translation submission.");
    }
}
