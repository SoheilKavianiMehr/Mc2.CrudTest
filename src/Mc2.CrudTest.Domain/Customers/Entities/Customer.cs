using Mc2.CrudTest.Domain.Basic.Common;
using Mc2.CrudTest.Domain.Customers.Events;
using Mc2.CrudTest.Domain.Customers.ValueObjects;

namespace Mc2.CrudTest.Domain.Customers.Entities;

public class Customer : AggregateRoot
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public DateTime DateOfBirth { get; private set; }
    public Email Email { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public string BankAccountNumber { get; private set; } = string.Empty;
    public bool IsDeleted { get; private set; }

    public string FullName => $"{FirstName} {LastName}";

    public string UniquenessKey => $"{FirstName.ToLowerInvariant()}|{LastName.ToLowerInvariant()}|{DateOfBirth:yyyy-MM-dd}";

    public int Age => CalculateAge(DateOfBirth);

    public Customer() { }

    public static Customer FromReadModel(
        Guid id,
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        string email,
        string phoneNumber,
        string bankAccountNumber,
        int version,
        bool isDeleted = false)
    {
        var emailValueObject = Email.Create(email);
        var phoneNumberValueObject = PhoneNumber.Create(phoneNumber);

        var customer = new Customer
        {
            Id = id,
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            DateOfBirth = dateOfBirth,
            Email = emailValueObject,
            PhoneNumber = phoneNumberValueObject,
            BankAccountNumber = bankAccountNumber.Trim(),
            Version = version,
            IsDeleted = isDeleted
        };

        return customer;
    }

    private Customer(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        Email email,
        PhoneNumber phoneNumber,
        string bankAccountNumber)
    {
        ValidateCustomerData(firstName, lastName, dateOfBirth, bankAccountNumber);

        Id = Guid.NewGuid();
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        DateOfBirth = dateOfBirth;
        Email = email;
        PhoneNumber = phoneNumber;
        BankAccountNumber = bankAccountNumber.Trim();

        ApplyEvent(new CustomerCreatedEvent(Id, FirstName, LastName, DateOfBirth, PhoneNumber.Value, Email.Value, BankAccountNumber));
    }

    public static Customer Create(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        string email,
        string phoneNumber,
        string bankAccountNumber)
    {
        var emailValueObject = Email.Create(email);
        var phoneNumberValueObject = PhoneNumber.Create(phoneNumber);

        return new Customer(
            firstName,
            lastName,
            dateOfBirth,
            emailValueObject,
            phoneNumberValueObject,
            bankAccountNumber);
    }

    public void Update(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        string email,
        string phoneNumber,
        string bankAccountNumber)
    {
        ValidateCustomerData(firstName, lastName, dateOfBirth, bankAccountNumber);

        var emailValueObject = Email.Create(email);
        var phoneNumberValueObject = PhoneNumber.Create(phoneNumber);

        var oldFullName = FullName;
        var oldEmail = Email.Value;

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        DateOfBirth = dateOfBirth;
        Email = emailValueObject;
        PhoneNumber = phoneNumberValueObject;
        BankAccountNumber = bankAccountNumber.Trim();

        ApplyEvent(new CustomerUpdatedEvent(Id, FirstName, LastName, DateOfBirth, PhoneNumber.Value, Email.Value, BankAccountNumber));
    }

    public void Delete()
    {
        ApplyEvent(new CustomerDeletedEvent(Id));
    }

    private static void ValidateCustomerData(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        string bankAccountNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));

        if (firstName.Trim().Length < 2 || firstName.Trim().Length > 50)
            throw new ArgumentException("First name must be between 2 and 50 characters.", nameof(firstName));

        if (lastName.Trim().Length < 2 || lastName.Trim().Length > 50)
            throw new ArgumentException("Last name must be between 2 and 50 characters.", nameof(lastName));

        if (dateOfBirth >= DateTime.Today)
            throw new ArgumentException("Date of birth must be in the past.", nameof(dateOfBirth));

        var age = CalculateAge(dateOfBirth);

        if (age < 18)
            throw new ArgumentException("Customer must be at least 18 years old.", nameof(dateOfBirth));

        if (age > 120)
            throw new ArgumentException("Invalid date of birth - age cannot exceed 120 years.", nameof(dateOfBirth));

        if (string.IsNullOrWhiteSpace(bankAccountNumber))
            throw new ArgumentException("Bank account number cannot be null or empty.", nameof(bankAccountNumber));

        if (bankAccountNumber.Trim().Length < 8 || bankAccountNumber.Trim().Length > 20)
            throw new ArgumentException("Bank account number must be between 8 and 34 characters.", nameof(bankAccountNumber));

        if (!bankAccountNumber.All(char.IsDigit))
            throw new ArgumentException("Bank account number must digits only.", nameof(bankAccountNumber));

    }

    private static int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;

        if (birthDate.Date > today.AddYears(-age))
            age--;

        return age;
    }

    public void Apply(CustomerCreatedEvent @event)
    {
        Id = @event.CustomerId;
        FirstName = @event.FirstName;
        LastName = @event.LastName;
        DateOfBirth = @event.DateOfBirth;
        PhoneNumber = PhoneNumber.Create(@event.PhoneNumber);
        Email = Email.Create(@event.Email);
        BankAccountNumber = @event.BankAccountNumber;
    }

    public void Apply(CustomerUpdatedEvent @event)
    {
        FirstName = @event.FirstName;
        LastName = @event.LastName;
        DateOfBirth = @event.DateOfBirth;
        PhoneNumber = PhoneNumber.Create(@event.PhoneNumber);
        Email = Email.Create(@event.Email);
        BankAccountNumber = @event.BankAccountNumber;
    }

    public void Apply(CustomerDeletedEvent @event)
    {
        IsDeleted = true;
    }
}
