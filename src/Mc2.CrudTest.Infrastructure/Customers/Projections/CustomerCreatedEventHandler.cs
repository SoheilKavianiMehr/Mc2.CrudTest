using Mc2.CrudTest.Domain.Customers.Events;
using Mc2.CrudTest.Infrastructure.Customers.ReadModels;
using Mc2.CrudTest.Infrastructure.Data;
using Mc2.CrudTest.Infrastructure.Projections;
using Microsoft.Extensions.Logging;

namespace Mc2.CrudTest.Infrastructure.Customers.Projections;

public class CustomerCreatedEventHandler : ProjectionHandlerBase<CustomerCreatedEvent>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CustomerCreatedEventHandler> _logger;

    public CustomerCreatedEventHandler(ApplicationDbContext context, ILogger<CustomerCreatedEventHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public override async Task HandleAsync(CustomerCreatedEvent @event)
    {
        _logger.LogDebug("Handling CustomerCreatedEvent for customer {CustomerId}", @event.CustomerId);

        var readModel = new CustomerReadModel
        {
            Id = @event.CustomerId,
            FirstName = @event.FirstName,
            LastName = @event.LastName,
            DateOfBirth = @event.DateOfBirth,
            PhoneNumber = @event.PhoneNumber,
            Email = @event.Email,
            BankAccountNumber = @event.BankAccountNumber,
            Version = @event.Version,
            IsDeleted = false,
            CreatedAt = @event.OccurredOn,
            UpdatedAt = null
        };

        _context.CustomerReadModels.Add(readModel);

        _logger.LogInformation("Customer read model created for customer {CustomerId}", @event.CustomerId);
    }
}