using Mc2.CrudTest.Application.Basic.Interfaces;
using Mc2.CrudTest.Domain.Customers;
using Mc2.CrudTest.Domain.Customers.Entities;
using Mc2.CrudTest.Infrastructure.Customers.ReadModels;
using Mc2.CrudTest.Infrastructure.Data;
using Mc2.CrudTest.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;

namespace Mc2.CrudTest.Infrastructure.Customers;

public class CustomerRepository : ICustomerRepository
{
    private readonly IEventStore _eventStore;
    private readonly ApplicationDbContext _context;
    private readonly IEventProjector _eventProjector;
    private readonly ITransactionManager _transactionManager;

    public CustomerRepository(IEventStore eventStore, ApplicationDbContext context, IEventProjector eventProjector, ITransactionManager transactionManager)
    {
        _eventStore = eventStore;
        _context = context;
        _eventProjector = eventProjector;
        _transactionManager = transactionManager;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        var readModels = await _context.CustomerReadModels
            .Where(c => !c.IsDeleted)
            .ToListAsync();

        return readModels.Select(ConvertToCustomer).ToList();
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        var events = await _eventStore.GetEventsAsync(id);
        if (!events.Any())
            return null;

        var customer = new Customer();
        customer.LoadFromHistory(events);

        if (customer.IsDeleted)
            return null;

        return customer;
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        var readModel = await _context.CustomerReadModels
            .FirstOrDefaultAsync(c => c.Email == email && !c.IsDeleted);

        return readModel != null ? ConvertToCustomer(readModel) : null;
    }

    public async Task<Customer?> GetByPhoneNumberAsync(string phoneNumber)
    {
        var readModel = await _context.CustomerReadModels
            .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber && !c.IsDeleted);

        return readModel != null ? ConvertToCustomer(readModel) : null;
    }

    public async Task<Customer?> GetByNameAndDateOfBirthAsync(string firstName, string lastName, DateTime dateOfBirth)
    {
        var readModel = await _context.CustomerReadModels
            .FirstOrDefaultAsync(c =>
                c.FirstName == firstName &&
                c.LastName == lastName &&
                c.DateOfBirth.Date == dateOfBirth.Date &&
                !c.IsDeleted);

        return readModel != null ? ConvertToCustomer(readModel) : null;
    }


    public async Task<Customer> CreateAsync(Customer customer)
    {
        using var transaction = await _transactionManager.BeginTransactionAsync();
        try
        {
            await _eventStore.SaveEventsAsync(
                customer.Id,
                nameof(Customer),
                customer.DomainEvents,
                0);

            await _eventProjector.ProjectAsync(customer.DomainEvents);

            await transaction.CommitAsync();
            customer.ClearDomainEvents();
            return customer;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Customer> UpdateAsync(Customer customer)
    {
        using var transaction = await _transactionManager.BeginTransactionAsync();
        try
        {
            await _eventStore.SaveEventsAsync(
                customer.Id,
                nameof(Customer),
                customer.DomainEvents,
                customer.Version - customer.DomainEvents.Count);

            await _eventProjector.ProjectAsync(customer.DomainEvents);

            await transaction.CommitAsync();
            customer.ClearDomainEvents();
            return customer;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteAsync(Customer customer)
    {
        customer.Delete();
        using var transaction = await _transactionManager.BeginTransactionAsync();
        try
        {
            await _eventStore.SaveEventsAsync(
                customer.Id,
                nameof(Customer),
                customer.DomainEvents,
                customer.Version - customer.DomainEvents.Count);

            await _eventProjector.ProjectAsync(customer.DomainEvents);

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task SaveChangesAsync()
    {
        await Task.CompletedTask;
    }

    public async Task<(IEnumerable<Customer> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.CustomerReadModels
            .Where(c => !c.IsDeleted)
            .CountAsync();

        var readModels = await _context.CustomerReadModels
            .Where(c => !c.IsDeleted)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var items = readModels.Select(ConvertToCustomer).ToList();
        return (items, totalCount);
    }

    public async Task<bool> IsExist(Customer customer)
    {
        return await _context.CustomerReadModels
            .AnyAsync(c => c.Id == customer.Id && !c.IsDeleted);
    }

    private Customer ConvertToCustomer(CustomerReadModel readModel)
    {
        var customer = Customer.FromReadModel(
            readModel.Id,
            readModel.FirstName,
            readModel.LastName,
            readModel.DateOfBirth,
            readModel.Email,
            readModel.PhoneNumber,
            readModel.BankAccountNumber,
            readModel.Version,
            readModel.IsDeleted);

        customer.ClearDomainEvents();
        return customer;
    }
}