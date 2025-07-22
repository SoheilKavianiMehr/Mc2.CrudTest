using Mc2.CrudTest.Domain.Common;
using Mc2.CrudTest.Domain.Events;
using Mc2.CrudTest.Domain.ValueObjects;

namespace Mc2.CrudTest.Domain.Entities;

public class Customer : BaseEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public DateTime DateOfBirth { get; private set; }
    public Email Email { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public string BankAccountNumber { get; private set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";

    public string UniquenessKey => $"{FirstName.ToLowerInvariant()}|{LastName.ToLowerInvariant()}|{DateOfBirth:yyyy-MM-dd}";

    private Customer() { }

    private Customer(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        Email email,
        PhoneNumber phoneNumber,
        string bankAccountNumber)
    {
        ValidateCustomerData(firstName, lastName, dateOfBirth, bankAccountNumber);

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        DateOfBirth = dateOfBirth;
        Email = email;
        PhoneNumber = phoneNumber;
        BankAccountNumber = bankAccountNumber.Trim();

        AddDomainEvent(new CustomerCreatedEvent(Id, FullName, Email.Value));
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

        MarkAsUpdated();
        AddDomainEvent(new CustomerUpdatedEvent(Id, oldFullName, FullName, oldEmail, Email.Value));
    }

    public void Delete()
    {
        MarkAsDeleted();
        AddDomainEvent(new CustomerDeletedEvent(Id, FullName, Email.Value));
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

        var age = DateTime.Today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;

        if (age < 18)
            throw new ArgumentException("Customer must be at least 18 years old.", nameof(dateOfBirth));

        if (age > 120)
            throw new ArgumentException("Invalid date of birth - age cannot exceed 120 years.", nameof(dateOfBirth));

        if (string.IsNullOrWhiteSpace(bankAccountNumber))
            throw new ArgumentException("Bank account number cannot be null or empty.", nameof(bankAccountNumber));

        if (bankAccountNumber.Trim().Length < 8 || bankAccountNumber.Trim().Length > 34)
            throw new ArgumentException("Bank account number must be between 8 and 34 characters.", nameof(bankAccountNumber));
    }
}
