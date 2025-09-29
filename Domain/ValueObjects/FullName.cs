namespace BankingSystem.Domain.ValueObjects
{
    public record FullName
    {
        public string FirstName { get; private init; }

        public string LastName { get; private init; }

        public string MiddleName { get; private init; }

        private FullName()
        {
            
        }

        public FullName(string firstName, string lastName, string middleName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Имя не может быть пустым", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Фамилия не может быть пустой", nameof(lastName));

            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(MiddleName)
                ? $"{FirstName} {LastName}"
                : $"{FirstName} {MiddleName} {LastName}";
        }
    }
}
