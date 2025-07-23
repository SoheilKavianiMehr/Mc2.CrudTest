using Mc2.CrudTest.Domain.Basic.Events;

namespace Mc2.CrudTest.Infrastructure.Projections;

public abstract class ProjectionHandlerBase<TEvent> : IProjectionHandler<TEvent> where TEvent : DomainEvent
{
    public bool CanHandle(Type eventType)
    {
        return typeof(TEvent).IsAssignableFrom(eventType);
    }

    public async Task HandleAsync(DomainEvent @event)
    {
        if (@event is TEvent typedEvent)
        {
            await HandleAsync(typedEvent);
        }
        else
        {
            throw new ArgumentException($"Event type {@event.GetType().Name} is not compatible with handler for {typeof(TEvent).Name}");
        }
    }

    public abstract Task HandleAsync(TEvent @event);
}