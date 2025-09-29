using System.Text.RegularExpressions;

namespace BankingSystem.Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; private init; }

        private Email()
        {
            
        }

        public Email(string value) 
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email не может быть пустым", nameof(value));

            if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Некорректный формат email", nameof(value));

            Value = value;
        }
    }
}
