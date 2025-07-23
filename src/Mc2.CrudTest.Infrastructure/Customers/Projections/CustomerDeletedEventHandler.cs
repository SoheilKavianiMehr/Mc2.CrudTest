using Mc2.CrudTest.Domain.Customers.Events;
using Mc2.CrudTest.Infrastructure.Data;
using Mc2.CrudTest.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mc2.CrudTest.Infrastructure.Customers.Projections;

public class CustomerDeletedEventHandler : ProjectionHandlerBase<CustomerDeletedEvent>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CustomerDeletedEventHandler> _logger;

    public CustomerDeletedEventHandler(ApplicationDbContext context, ILogger<CustomerDeletedEventHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public override async Task HandleAsync(CustomerDeletedEvent @event)
    {
        _logger.LogDebug("Handling CustomerDeletedEvent for customer {CustomerId}", @event.CustomerId);

        var readModel = await _context.CustomerReadModels
            .FirstOrDefaultAsync(c => c.Id == @event.CustomerId);

        if (readModel != null)
        {
            readModel.IsDeleted = true;
            readModel.Version = @event.Version;
            readModel.UpdatedAt = @event.OccurredOn;

            _logger.LogInformation("Customer read model marked as deleted for customer {CustomerId}", @event.CustomerId);
        }
        else
        {
            _logger.LogWarning("Customer read model not found for delete event. CustomerId: {CustomerId}", @event.CustomerId);
        }
    }
}