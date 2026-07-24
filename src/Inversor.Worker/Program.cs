
using Inversor.Core;
using Inversor.Infrastructure;
using Inversor.Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, massTransit =>
{
    massTransit.AddConsumer<EvaluateTranslationConsumer>();
});

var host = builder.Build();
host.Run();
