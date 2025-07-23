using Mc2.CrudTest.Domain.Basic.Events;

namespace Mc2.CrudTest.Domain.Customers.Events;

public class CustomerCreatedEvent : DomainEvent
{
    public Guid CustomerId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public DateTime DateOfBirth { get; }
    public string PhoneNumber { get; }
    public string Email { get; }
    public string BankAccountNumber { get; }

    public CustomerCreatedEvent(
        Guid customerId,
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        string phoneNumber,
        string email,
        string bankAccountNumber)
    {
        CustomerId = customerId;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        PhoneNumber = phoneNumber;
        Email = email;
        BankAccountNumber = bankAccountNumber;
    }
}
