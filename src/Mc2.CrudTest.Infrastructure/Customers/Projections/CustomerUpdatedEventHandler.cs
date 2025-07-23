using Mc2.CrudTest.Domain.Customers.Events;
using Mc2.CrudTest.Infrastructure.Data;
using Mc2.CrudTest.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mc2.CrudTest.Infrastructure.Customers.Projections;

public class CustomerUpdatedEventHandler : ProjectionHandlerBase<CustomerUpdatedEvent>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CustomerUpdatedEventHandler> _logger;

    public CustomerUpdatedEventHandler(ApplicationDbContext context, ILogger<CustomerUpdatedEventHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public override async Task HandleAsync(CustomerUpdatedEvent @event)
    {
        _logger.LogDebug("Handling CustomerUpdatedEvent for customer {CustomerId}", @event.CustomerId);

        var readModel = await _context.CustomerReadModels
            .FirstOrDefaultAsync(c => c.Id == @event.CustomerId);

        if (readModel != null)
        {
            readModel.FirstName = @event.FirstName;
            readModel.LastName = @event.LastName;
            readModel.DateOfBirth = @event.DateOfBirth;
            readModel.PhoneNumber = @event.PhoneNumber;
            readModel.Email = @event.Email;
            readModel.BankAccountNumber = @event.BankAccountNumber;
            readModel.Version = @event.Version;
            readModel.UpdatedAt = @event.OccurredOn;

            _logger.LogInformation("Customer read model updated for customer {CustomerId}", @event.CustomerId);
        }
        else
        {
            _logger.LogWarning("Customer read model not found for update event. CustomerId: {CustomerId}", @event.CustomerId);
        }
    }
}