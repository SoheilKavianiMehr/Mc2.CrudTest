using System;

namespace Mc2.CrudTest.Domain.Basic.Common;

public abstract class BaseDomainEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public abstract string EventType { get; }
}