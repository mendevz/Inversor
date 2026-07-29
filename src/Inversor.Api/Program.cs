using Inversor.Api.Consumers;
using Inversor.Api.Endpoints;
using Inversor.Api.Extensions;
using Inversor.Api.Hubs;
using Inversor.Api.Middlewares;
using Inversor.Core;
using Inversor.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration, x =>
{
    x.AddConsumer<EvaluationCompletedConsumer>();
});

builder.Services.AddSignalR();
builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

if (args.Contains("--only-migrate"))
{
    await app.ApplyProjectMigrationsAsync();
    return; 
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Inversor API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.MapEvaluationEndpoints();

app.MapHub<NotificationHub>("/hubs/notifications");

app.UseHttpsRedirection();

app.Run();