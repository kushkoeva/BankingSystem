using BankingSystem.Domain.Entities;

namespace BankingSystem.Worker.Services
{
    internal interface ICalculationService
    {
        Task CalculateAsync(Transaction transaction);
    }
}
