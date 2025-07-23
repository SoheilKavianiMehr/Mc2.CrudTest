namespace Mc2.CrudTest.Domain.Basic.Events;

public abstract class DomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string EventType { get; } = string.Empty;
    public int Version { get; set; } = 1;

    protected DomainEvent()
    {
        EventType = GetType().Name;
    }
}