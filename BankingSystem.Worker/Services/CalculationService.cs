using BankingSystem.Domain.Entities;

namespace BankingSystem.Worker.Services
{
    internal class CalculationService : ICalculationService
    {
        public async Task CalculateAsync(Transaction transaction)
        {
            await Task.CompletedTask;
        }
    }
}
