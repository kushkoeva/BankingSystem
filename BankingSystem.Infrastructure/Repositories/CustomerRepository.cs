using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return customer.CustomerId;
        }

        public async Task<Customer> GetByIdAsync(Guid customerId) => await _context.Customers
                .Include(c => c.Accounts)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }
}
