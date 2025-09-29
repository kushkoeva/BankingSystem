using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Domain.Entities
{
    public class Transaction
    {
        public Guid TransactionId { get; private set; }
        public Guid AccountId { get; private set; }
        public Money Money { get; private set; }
        public string Type { get; private set; }
        public DateTime CreateDate { get; private set; }

        private Transaction() { }

        public Transaction(Guid accountId, Money money, TransactionType type)
        {
            TransactionId = Guid.NewGuid();
            AccountId = accountId;
            Money = money;
            Type = type.ToString();
            CreateDate = DateTime.UtcNow;
        }
    }

    public enum TransactionType
    {
        Initial,
        Deposit,
        Withdrawal
    }
}
