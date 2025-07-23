using Mc2.CrudTest.Domain.Basic.Events;

namespace Mc2.CrudTest.Application.Basic.Interfaces;

public interface IEventStore
{
    Task SaveEventsAsync(Guid aggregateId, string aggregateType, IEnumerable<DomainEvent> events, int expectedVersion);
    Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId);
    Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId, int fromVersion);
    Task<IEnumerable<Guid>> GetAllAggregateIdsAsync(string aggregateType);
}