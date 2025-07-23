using Mc2.CrudTest.Domain.Customers.Entities;

namespace Mc2.CrudTest.Domain.Customers;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(Guid id);
    Task<Customer?> GetByEmailAsync(string email);
    Task<Customer?> GetByPhoneNumberAsync(string phoneNumber);
    Task<Customer?> GetByNameAndDateOfBirthAsync(string firstName, string lastName, DateTime dateOfBirth);
    Task<bool> IsExist(Customer customer);
    Task<Customer> CreateAsync(Customer customer);
    Task<Customer> UpdateAsync(Customer customer);
    Task DeleteAsync(Customer customer);
    Task SaveChangesAsync();
    Task<(IEnumerable<Customer> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
}