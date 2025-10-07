using System.Collections.Concurrent;

namespace BankingSystem.Infrastructure.Services
{
    public class CurrencyRateService: ICurrencyRateService
    {
        private ConcurrentDictionary<(string Base, string Target), decimal> _rates;

        public CurrencyRateService()
        {
            _rates.TryAdd(("USD", "EUR"), 0.93m);
            _rates.TryAdd(("EUR", "USD"), 1.075m);

            _rates.TryAdd(("USD", "RUB"), 98.50m);
            _rates.TryAdd(("RUB", "USD"), 0.0102m);

            _rates.TryAdd(("EUR", "RUB"), 106.00m);
            _rates.TryAdd(("RUB", "EUR"), 0.0094m);
        }

        public bool TryGetRate(string baseCurrency, string targetCurrency, out decimal rate)
        {
            return _rates.TryGetValue((baseCurrency, targetCurrency), out rate);
        }

        public void SetRate(string baseCurrency, string targetCurrency, decimal rate)
        {
            _rates[(baseCurrency, targetCurrency)] = rate;
        }

        public bool TryUpdateRate(string baseCurrency, string targetCurrency, decimal newRate)
        {
            return _rates.TryGetValue((baseCurrency, targetCurrency), out var oldRate)
                && _rates.TryUpdate((baseCurrency, targetCurrency), newRate, oldRate);
        }

        public IReadOnlyDictionary<(string Base, string Target), decimal> GetAllRates()
        {
            return new Dictionary<(string Base, string Target), decimal>(_rates);
        }
    }
}
