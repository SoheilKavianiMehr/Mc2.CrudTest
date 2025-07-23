using Mc2.CrudTest.Domain.Basic.Events;

namespace Mc2.CrudTest.Infrastructure.Projections;

public interface IEventProjector
{
    Task ProjectAsync(DomainEvent @event);
    Task ProjectAsync(IEnumerable<DomainEvent> events);
}