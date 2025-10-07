namespace BankingSystem.Infrastructure.Services
{
    public interface ICurrencyRateService
    {
        bool TryGetRate(string baseCurrency, string targetCurrency, out decimal rate);
        void SetRate(string baseCurrency, string targetCurrency, decimal rate);
        bool TryUpdateRate(string baseCurrency, string targetCurrency, decimal newRate);
        IReadOnlyDictionary<(string Base, string Target), decimal> GetAllRates();
    }
}
