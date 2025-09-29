namespace BankingSystem.Domain.ValueObjects
{
    public record Address
    {
        public string Country { get; private init; }
        public string Region { get; private init; }
        public string City { get; private init; }
        public string Street { get; private init; }
        public string House { get; private init; }
        public string ZipCode { get; private init; }

        private Address()
        {
            
        }

        public Address(string country, string region, string city, string street, string house, string zipCode)
        {
            Country = country;
            Region = region;
            City = city;
            Street = street;
            House = house;
            ZipCode = zipCode;
        }
    }
}
