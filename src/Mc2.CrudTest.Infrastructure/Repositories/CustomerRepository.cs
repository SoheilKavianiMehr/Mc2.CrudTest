using Microsoft.EntityFrameworkCore;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Domain.Repositories;
using Mc2.CrudTest.Infrastructure.Data;

namespace Mc2.CrudTest.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer> UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task DeleteAsync(Customer customer)
    {
        _context.Customers.Remove(customer);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<Customer> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var totalCount = await _context.Customers.CountAsync();
        var items = await _context.Customers
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<bool> IsExist(Customer customer)
    {
        return await _context.Customers
            .AnyAsync(a => 
        (a.FirstName.Equals(customer.FirstName) &&
        a.LastName.Equals(customer.LastName) &&
        a.DateOfBirth == customer.DateOfBirth) ||
        a.Email.Equals(customer.Email));
    }
}