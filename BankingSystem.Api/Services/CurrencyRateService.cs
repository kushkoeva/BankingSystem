using BankingSystem.Infrastructure.Services;
using CurrencyRate;
using Grpc.Core;

namespace BankingSystem.API.Services
{
    public class CurrencyRateService: CurrencyRate.CurrencyRate.CurrencyRateBase
    {
        private readonly ICurrencyRateService _currencyRateService;
        private readonly ILogger _logger;
        public CurrencyRateService(
            ICurrencyRateService currencyRateService,
            ILogger logger)
        {
            _currencyRateService = currencyRateService;
            _logger = logger;
        }

        public override async Task<CurrencyUpdateResponse> UpdateCurrency(CurrencyUpdateRequest request, ServerCallContext context)
        {
            try
            {
                var isSuccess = _currencyRateService.TryUpdateRate(request.BaseCurrency, request.TargetCurrency, decimal.Parse(request.Amount));

                return new CurrencyUpdateResponse
                {
                    Success = isSuccess,
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new CurrencyUpdateResponse
                {
                    Success = false,
                };
            }
        }
    }
}
