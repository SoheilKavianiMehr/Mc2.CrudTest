using Mc2.CrudTest.Domain.Common;

namespace Mc2.CrudTest.Domain.Events;

public class CustomerCreatedEvent : BaseDomainEvent
{
    public Guid CustomerId { get; }
    public string CustomerName { get; }
    public string Email { get; }

    public CustomerCreatedEvent(Guid customerId, string customerName, string email)
    {
        CustomerId = customerId;
        CustomerName = customerName;
        Email = email;
    }

    public override string EventType => nameof(CustomerCreatedEvent);
}

public class CustomerUpdatedEvent : BaseDomainEvent
{
    public Guid CustomerId { get; }
    public string OldCustomerName { get; }
    public string NewCustomerName { get; }
    public string OldEmail { get; }
    public string NewEmail { get; }

    public CustomerUpdatedEvent(
        Guid customerId,
        string oldCustomerName,
        string newCustomerName,
        string oldEmail,
        string newEmail)
    {
        CustomerId = customerId;
        OldCustomerName = oldCustomerName;
        NewCustomerName = newCustomerName;
        OldEmail = oldEmail;
        NewEmail = newEmail;
    }

    public override string EventType => nameof(CustomerUpdatedEvent);
}

public class CustomerDeletedEvent : BaseDomainEvent
{
    public Guid CustomerId { get; }
    public string CustomerName { get; }
    public string Email { get; }

    public CustomerDeletedEvent(Guid customerId, string customerName, string email)
    {
        CustomerId = customerId;
        CustomerName = customerName;
        Email = email;
    }

    public override string EventType => nameof(CustomerDeletedEvent);
}