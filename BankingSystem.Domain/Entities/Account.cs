using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Domain.Entities
{
    public class Account
    {
        public Guid AccountId { get; private set; }
        public Money Balance { get; private set; }
        public Guid CustomerId { get; private set; }

        public Customer Customer { get; private set; }

        private readonly List<Transaction> _transactions = new();
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        private Account() { } 

        public Account(Guid customerId, Money initialDeposit)
        {
            AccountId = Guid.NewGuid();
            CustomerId = customerId;
            Balance = initialDeposit;
            if (initialDeposit.Amount > 0)
            {
                var deposit = new Transaction(AccountId, initialDeposit, TransactionType.Initial);
                _transactions.Add(deposit);
            }
        }

        public void Deposit(Money amount)
        {
            Balance += amount;
            _transactions.Add(new Transaction(AccountId, amount, TransactionType.Deposit));
        }

        public void Withdraw(Money amount)
        {
            if (Balance.Amount < amount.Amount)
            {
                throw new InvalidOperationException("Недостаточно средств");
            }

            Balance -= amount;
            _transactions.Add(new Transaction(AccountId, amount, TransactionType.Withdrawal));
        }

        public void Transfer(Account target, Money amount)
        {
            Withdraw(amount);
            target.Deposit(amount);
        }
    }
}
