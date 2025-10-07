namespace BankingSystem.Domain.ValueObjects
{
    public record PhoneNumber
    {
        public string Value { get; private init; }

        private PhoneNumber()
        {
            
        }

        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Номер телефона не может быть пустой", nameof(value));
            }

            Value = value;
        }
    }
}
