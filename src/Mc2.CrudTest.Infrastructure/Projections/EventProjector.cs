using Mc2.CrudTest.Domain.Basic.Events;
using Mc2.CrudTest.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Mc2.CrudTest.Infrastructure.Projections;

public class EventProjector : IEventProjector
{
    private readonly IEnumerable<IProjectionHandler> _handlers;
    private readonly ILogger<EventProjector> _logger;
    private readonly ApplicationDbContext _context;

    public EventProjector(IEnumerable<IProjectionHandler> handlers, ILogger<EventProjector> logger, ApplicationDbContext context)
    {
        _handlers = handlers;
        _logger = logger;
        _context = context;
    }

    public async Task ProjectAsync(DomainEvent @event)
    {
        var eventType = @event.GetType();
        var handlers = _handlers.Where(h => h.CanHandle(eventType)).ToList();

        if (!handlers.Any())
        {
            _logger.LogWarning("No projection handler found for event type {EventType}", eventType.Name);
            return;
        }

        foreach (var handler in handlers)
        {
            try
            {
                await handler.HandleAsync(@event);
                _logger.LogDebug("Successfully projected event {EventType} using handler {HandlerType}", 
                    eventType.Name, handler.GetType().Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error projecting event {EventType} using handler {HandlerType}", 
                    eventType.Name, handler.GetType().Name);
                throw;
            }
        }
    }

    public async Task ProjectAsync(IEnumerable<DomainEvent> events)
    {
        var eventsList = events.ToList();
        if (!eventsList.Any())
            return;

        var handledEvents = new List<(DomainEvent Event, IProjectionHandler Handler)>();

        foreach (var @event in eventsList)
        {
            var eventType = @event.GetType();
            var handlers = _handlers.Where(h => h.CanHandle(eventType)).ToList();

            if (!handlers.Any())
            {
                _logger.LogWarning("No projection handler found for event type {EventType}", eventType.Name);
                continue;
            }

            foreach (var handler in handlers)
            {
                handledEvents.Add((@event, handler));
            }
        }

        foreach (var (eventItem, handler) in handledEvents)
        {
            try
            {
                await handler.HandleAsync(eventItem);
                _logger.LogDebug("Successfully projected event {EventType} using handler {HandlerType}", 
                    eventItem.GetType().Name, handler.GetType().Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error projecting event {EventType} using handler {HandlerType}", 
                    eventItem.GetType().Name, handler.GetType().Name);
                throw;
            }
        }

        if (handledEvents.Any())
        {
            await _context.SaveChangesAsync();
        }
    }
}