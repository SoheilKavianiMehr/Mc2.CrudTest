using Mc2.CrudTest.Domain.Entities;

namespace Mc2.CrudTest.Domain.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(Guid id);
    Task<Customer?> GetByEmailAsync(string email);
    Task<bool> IsExist(Customer customer);
    Task<Customer> CreateAsync(Customer customer);
    Task<Customer> UpdateAsync(Customer customer);
    Task DeleteAsync(Customer customer);
    Task SaveChangesAsync();
    Task<(IEnumerable<Customer> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
}