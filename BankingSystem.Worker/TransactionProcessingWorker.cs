using BankingSystem.Infrastructure;
using BankingSystem.Worker.Services;
using Cronos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BankingSystem.Worker
{
    internal class TransactionProcessingWorker : BackgroundService
    {
        private readonly ILogger<TransactionProcessingWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TransactionWorkerConfiguration _transactionWorkerConfiguration;

        public TransactionProcessingWorker(
            ILogger<TransactionProcessingWorker> logger,
            IServiceProvider serviceProvider,
            IOptionsMonitor<TransactionWorkerConfiguration> transactionWorkerOptions)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _transactionWorkerConfiguration = transactionWorkerOptions.CurrentValue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(TransactionProcessingWorker)} запущен");
            var cronExpression = CronExpression.Parse(_transactionWorkerConfiguration.Schedule, CronFormat.IncludeSeconds);
            var nextRun = cronExpression.GetNextOccurrence(DateTime.UtcNow);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (DateTimeOffset.UtcNow >= nextRun)
                    {
                        var db = _serviceProvider.GetRequiredService<AppDbContext>();
                        var calculator = _serviceProvider.GetRequiredService<ICalculationService>();

                        var pendingTransactions = await db.Transactions
                            .Where(t => !t.IsProcessed)
                            .Take(100)
                            .ToListAsync(stoppingToken);

                        _logger.LogInformation($"Обработка {pendingTransactions.Count} транзакций");

                        var parallelOptions = new ParallelOptions
                        {
                            MaxDegreeOfParallelism = Environment.ProcessorCount,
                            CancellationToken = stoppingToken
                        };

                        await Parallel.ForEachAsync(pendingTransactions, parallelOptions, async (transaction, token) =>
                        {
                            try
                            {
                                calculator.CalculateAsync(transaction);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"Ошибка расчета транзакции {transaction.TransactionId}");
                            }
                        });

                        await db.SaveChangesAsync(stoppingToken);
                        nextRun = cronExpression.GetNextOccurrence(DateTime.UtcNow);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{nameof(TransactionProcessingWorker)} завершился с ошибкой");
                }
            }
        }
    }
}
