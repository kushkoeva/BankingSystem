namespace BankingSystem.Domain.ValueObjects
{
    public record Money
    {
        public decimal Amount { get; private init; }
        public string Currency { get; private init; }

        public Money(decimal amount, string currency)
        {
            if (amount < 0)
                throw new ArgumentException("Сумма не может быть негативной", nameof(amount));
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Валюта не может быть пустой", nameof(currency));

            Amount = amount;
            Currency = currency.ToUpperInvariant();
        }

        public static Money operator +(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Нельзя складывать разные валюты");
            return new Money(a.Amount + b.Amount, a.Currency);
        }

        public static Money operator -(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Нельзя вычитать разные валюты");
            return new Money(a.Amount - b.Amount, a.Currency);
        }
    }
}
