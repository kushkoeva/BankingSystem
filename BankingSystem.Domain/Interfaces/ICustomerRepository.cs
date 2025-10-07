using BankingSystem.Domain.Entities;

namespace BankingSystem.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(Guid customerId);

        Task<Guid> AddAsync(Customer customer);
    }
}
