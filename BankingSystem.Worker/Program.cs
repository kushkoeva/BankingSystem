using BankingSystem.Worker;
using BankingSystem.Worker.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<TransactionProcessingWorker>();
builder.Services.AddSingleton<ICalculationService, CalculationService>();

var host = builder.Build();
host.Run();