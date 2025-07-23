using Mc2.CrudTest.Domain.Basic.Events;

namespace Mc2.CrudTest.Domain.Customers.Events;

public class CustomerDeletedEvent : DomainEvent
{
    public Guid CustomerId { get; }

    public CustomerDeletedEvent(Guid customerId)
    {
        CustomerId = customerId;
    }
}