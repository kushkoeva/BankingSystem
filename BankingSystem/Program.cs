using BankingSystem.API.Services;
using BankingSystem.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

builder.Services.AddGrpc();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapGrpcService<BankingSystemService>();

app.Run();
