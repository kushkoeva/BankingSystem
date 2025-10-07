using BankingSystem.Domain.Entities;

namespace BankingSystem.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetByIdAsync(Guid accountId);
        Task<Guid> AddAsync(Account account);
        Task UpdateAsync(Account account);

        Task<IReadOnlyCollection<Transaction>> GetTransactionsAsync(Guid accountId, int pageNumber, int pageSize);
    }
}
