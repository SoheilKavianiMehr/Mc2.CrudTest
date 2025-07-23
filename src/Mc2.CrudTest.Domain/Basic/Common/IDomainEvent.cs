using System;

namespace Mc2.CrudTest.Domain.Basic.Common;

public interface IDomainEvent
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
    string EventType { get; }
}