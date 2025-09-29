using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Domain.Entities
{
    public class Customer
    {
        public Guid CustomerId { get; private set; }

        public FullName Name { get; private set; }
        public Email Email { get; private set; }
        public PhoneNumber Phone { get; private set; }
        public Address Address { get; private set; }

        public CustomerStatus Status { get; private set; }

        private readonly List<Account> _accounts = new();
        public IReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();

        private Customer() { }

        public Customer(FullName name, Email email, PhoneNumber phone, Address address)
        {
            CustomerId = new Guid();
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
            Status = CustomerStatus.Active;
        }

        public void UpdateAddress(Address newAddress)
        {
            if (Status == CustomerStatus.Blocked)
            {
                throw new InvalidOperationException("Нельзя обновлять данные заблокированного клиента.");
            }

            Address = newAddress;
        }

        public void Block()
        {
            Status = CustomerStatus.Blocked;
        }

        public void Unblock()
        {
            Status = CustomerStatus.Active;
        }

        public void AddAccount(Account account)
        {
            if (Status != CustomerStatus.Active)
            {
                throw new InvalidOperationException("Нельзя открыть счёт для неактивного клиента.");
            }

            _accounts.Add(account);
        }
    }

    public enum CustomerStatus
    {
        Active,
        Blocked,
        Closed
    }
}
