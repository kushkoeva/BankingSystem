using System.Linq;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Account> GetByIdAsync(Guid accountId) => await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);

        public async Task<Guid> AddAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account.AccountId;
        }

        public async Task UpdateAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<Transaction>> GetTransactionsAsync(Guid accountId, int pageNumber, int pageSize)
        {
            var account = await _context.Accounts
            .Include(x => x.Transactions
                .OrderByDescending(t => t.CreateDate)
                .Skip(pageNumber * pageSize)
                .Take(pageSize))
            .FirstOrDefaultAsync(x => x.AccountId == accountId);

            return account.Transactions;
        }

                    
    }
}
