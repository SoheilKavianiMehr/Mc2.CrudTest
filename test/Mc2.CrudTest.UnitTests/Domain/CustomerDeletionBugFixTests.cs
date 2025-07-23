using Mc2.CrudTest.Domain.Customers.Entities;
using Mc2.CrudTest.Domain.Customers.Events;

namespace Mc2.CrudTest.UnitTests.Domain;

public class CustomerDeletionBugFixTests
{

    [Fact]
    public void Customer_WhenDeleted_ShouldHaveIsDeletedSetToTrue()
    {
        var customer = Customer.Create(
            "soheil",
            "km",
            new DateTime(1990, 1, 1),
            "soheil79km@hotmail.com",
            "+989383623312",
            "123456788899");

        customer.Delete();
        var deleteEvent = new CustomerDeletedEvent(customer.Id);
        customer.Apply(deleteEvent);

        Assert.True(customer.IsDeleted, "Customer should be marked as deleted after applying CustomerDeletedEvent");
    }

    [Fact]
    public void Customer_WhenCreated_ShouldHaveIsDeletedSetToFalse()
    {
        var customer = Customer.Create(
            "soheil",
            "km",
            new DateTime(1985, 5, 15),
            "soheil79km@hotmail.com",
            "+989383623312",
            "123456788899");

        Assert.False(customer.IsDeleted, "Newly created customer should not be marked as deleted");
    }

    [Fact]
    public void Customer_FromReadModel_ShouldPreserveIsDeletedState()
    {
        var customerId = Guid.NewGuid();
        var isDeleted = true;

        var customer = Customer.FromReadModel(
            customerId,
            "Test",
            "User",
            new DateTime(1990, 1, 1),
            "test@example.com",
            "+1234567890",
            "12345678901234567890",
            1,
            isDeleted);

        Assert.True(customer.IsDeleted, "Customer created from read model should preserve IsDeleted state");
        Assert.Equal(customerId, customer.Id);
    }



    [Fact]
    public void CustomerDeletedEvent_Apply_ShouldSetIsDeletedToTrue()
    {
        var customer = new Customer();
        var customerId = Guid.NewGuid();
        var deleteEvent = new CustomerDeletedEvent(customerId);

        Assert.False(customer.IsDeleted);

        customer.Apply(deleteEvent);

        Assert.True(customer.IsDeleted);
    }

    [Fact]
    public void Customer_EventSourcingLifecycle_ShouldMaintainCorrectIsDeletedState()
    {
        var customer = new Customer();
        var customerId = Guid.NewGuid();

        var createEvent = new CustomerCreatedEvent(
            customerId,
            "Lifecycle",
            "Test",
            new DateTime(1990, 1, 1),
            "+1234567890",
            "lifecycle@example.com",
            "12345678901234567890");

        var updateEvent = new CustomerUpdatedEvent(
            customerId,
            "Updated",
            "Test",
            new DateTime(1990, 1, 1),
            "+1234567890",
            "updated@example.com",
            "12345678901234567890");

        var deleteEvent = new CustomerDeletedEvent(customerId);

        customer.Apply(createEvent);
        Assert.False(customer.IsDeleted, "Customer should not be deleted after creation");

        customer.Apply(updateEvent);
        Assert.False(customer.IsDeleted, "Customer should not be deleted after update");

        customer.Apply(deleteEvent);
        Assert.True(customer.IsDeleted, "Customer should be deleted after delete event");
    }
}