using Mc2.CrudTest.Domain.Basic.Events;

namespace Mc2.CrudTest.Infrastructure.Projections;

public interface IProjectionHandler
{
    bool CanHandle(Type eventType);
    Task HandleAsync(DomainEvent @event);
}

public interface IProjectionHandler<in TEvent> : IProjectionHandler where TEvent : DomainEvent
{
    Task HandleAsync(TEvent @event);
}