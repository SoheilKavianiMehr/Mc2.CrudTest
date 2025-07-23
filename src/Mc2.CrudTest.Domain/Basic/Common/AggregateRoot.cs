using Mc2.CrudTest.Domain.Basic.Events;

namespace Mc2.CrudTest.Domain.Basic.Common;

public abstract class AggregateRoot
{
    private readonly List<DomainEvent> _domainEvents = new();

    public Guid Id { get; protected set; }
    public int Version { get; protected set; }

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        domainEvent.Version = Version + 1;
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void LoadFromHistory(IEnumerable<DomainEvent> events)
    {
        foreach (var @event in events.OrderBy(e => e.Version))
        {
            ApplyEvent(@event, false);
            Version = @event.Version;
        }
    }

    protected void ApplyEvent(DomainEvent @event, bool isNew = true)
    {
        var eventType = @event.GetType();
        var method = GetType().GetMethod("Apply", new[] { eventType });

        if (method == null)
        {
            throw new InvalidOperationException($"Apply method not found for event type {eventType.Name}");
        }

        method.Invoke(this, new object[] { @event });

        if (isNew)
        {
            Version++;
            @event.Version = Version;
            _domainEvents.Add(@event);
        }
    }
}