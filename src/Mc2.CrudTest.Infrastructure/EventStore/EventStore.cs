using Mc2.CrudTest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Reflection;
using Mc2.CrudTest.Application.Basic.Interfaces;
using Mc2.CrudTest.Domain.Basic.Events;

namespace Mc2.CrudTest.Infrastructure.EventStore;

public class EventStore : IEventStore
{
    private readonly ApplicationDbContext _context;
    private readonly JsonSerializerOptions _jsonOptions;

    public EventStore(ApplicationDbContext context)
    {
        _context = context;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public async Task SaveEventsAsync(Guid aggregateId, string aggregateType, IEnumerable<DomainEvent> events, int expectedVersion)
    {
        var eventEntities = new List<EventStoreEntity>();

        foreach (var @event in events)
        {
            var eventData = JsonSerializer.Serialize(@event, @event.GetType(), _jsonOptions);
            
            var eventEntity = new EventStoreEntity
            {
                Id = @event.Id,
                AggregateId = aggregateId,
                AggregateType = aggregateType,
                EventType = @event.EventType,
                EventData = eventData,
                Version = @event.Version,
                OccurredOn = @event.OccurredOn
            };

            eventEntities.Add(eventEntity);
        }

        _context.EventStore.AddRange(eventEntities);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId)
    {
        return await GetEventsAsync(aggregateId, 0);
    }

    public async Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId, int fromVersion)
    {
        var eventEntities = await _context.EventStore
            .Where(e => e.AggregateId == aggregateId && e.Version > fromVersion)
            .OrderBy(e => e.Version)
            .ToListAsync();

        var events = new List<DomainEvent>();

        foreach (var eventEntity in eventEntities)
        {
            var eventType = GetEventType(eventEntity.EventType);
            if (eventType != null)
            {
                var @event = JsonSerializer.Deserialize(eventEntity.EventData, eventType, _jsonOptions) as DomainEvent;
                if (@event != null)
                {
                    events.Add(@event);
                }
            }
        }

        return events;
    }

    public async Task<IEnumerable<Guid>> GetAllAggregateIdsAsync(string aggregateType)
    {
        return await _context.EventStore
            .Where(e => e.AggregateType == aggregateType)
            .Select(e => e.AggregateId)
            .Distinct()
            .ToListAsync();
    }

    private Type? GetEventType(string eventTypeName)
    {
        var assemblies = new[]
        {
            Assembly.GetAssembly(typeof(DomainEvent)),
            Assembly.GetExecutingAssembly()
        };

        foreach (var assembly in assemblies.Where(a => a != null))
        {
            var type = assembly!.GetTypes()
                .FirstOrDefault(t => t.Name == eventTypeName && typeof(DomainEvent).IsAssignableFrom(t));
            
            if (type != null)
                return type;
        }

        return null;
    }
}