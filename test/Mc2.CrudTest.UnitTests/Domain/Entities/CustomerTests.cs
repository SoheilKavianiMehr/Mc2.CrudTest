using FluentAssertions;
using Mc2.CrudTest.Domain.Customers.Entities;
using Mc2.CrudTest.Domain.Customers.Events;
using Mc2.CrudTest.Domain.Customers.ValueObjects;

namespace Mc2.CrudTest.UnitTests.Domain.Entities
{
    public class CustomerTests
    {
        private readonly string _validFirstName = "soheil";
        private readonly string _validLastName = "km";
        private readonly DateTime _validDateOfBirth = new DateTime(2000, 1, 15);
        private readonly Email _validEmail = Email.Create("soheilkm@hotmail.com");
        private readonly PhoneNumber _validPhoneNumber = PhoneNumber.Create("+989383623312");
        private readonly string _validBankAccountNumber = "12345678901234567890";

        [Fact]
        public void Create_WithValidData_ShouldCreateCustomerAndRaiseEvent()
        {
            var customer = Customer.Create(
                _validFirstName,
                _validLastName,
                _validDateOfBirth,
                _validEmail,
                _validPhoneNumber,
                _validBankAccountNumber);

            customer.Should().NotBeNull();
            customer.Id.Should().NotBeEmpty();
            customer.FirstName.Should().Be(_validFirstName);
            customer.LastName.Should().Be(_validLastName);
            customer.DateOfBirth.Should().Be(_validDateOfBirth);
            customer.Email.Should().Be(_validEmail);
            customer.PhoneNumber.Should().Be(_validPhoneNumber);
            customer.BankAccountNumber.Should().Be(_validBankAccountNumber);
            
            customer.DomainEvents.Should().HaveCount(1);
            customer.DomainEvents.First().Should().BeOfType<CustomerCreatedEvent>();
            var createdEvent = (CustomerCreatedEvent)customer.DomainEvents.First();
            createdEvent.CustomerId.Should().Be(customer.Id);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("A")]
        [InlineData("testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest")]
        public void Create_WithInvalidFirstName_ShouldThrowArgumentException(string invalidFirstName)
        {
            Action act = () => Customer.Create(
                invalidFirstName,
                _validLastName,
                _validDateOfBirth,
                _validEmail,
                _validPhoneNumber,
                _validBankAccountNumber);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*FirstName*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("A")]
        [InlineData("testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest")]
        public void Create_WithInvalidLastName_ShouldThrowArgumentException(string invalidLastName)
        {
            Action act = () => Customer.Create(
                _validFirstName,
                invalidLastName,
                _validDateOfBirth,
                _validEmail,
                _validPhoneNumber,
                _validBankAccountNumber);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*LastName*");
        }

        [Fact]
        public void Create_WithUnderageCustomer_ShouldThrowArgumentException()
        {
            var underageDate = DateTime.Now.AddYears(-17);

            Action act = () => Customer.Create(
                _validFirstName,
                _validLastName,
                underageDate,
                _validEmail,
                _validPhoneNumber,
                _validBankAccountNumber);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*at*least*18*");
        }

        [Fact]
        public void Create_WithFutureDateOfBirth_ShouldThrowArgumentException()
        {
            var futureDate = DateTime.Now.AddDays(1);

            Action act = () => Customer.Create(
                _validFirstName,
                _validLastName,
                futureDate,
                _validEmail,
                _validPhoneNumber,
                _validBankAccountNumber);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*must*be*in*the*past*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("123")]
        [InlineData("123456789012345678901")]
        [InlineData("abcd1234567890123456")]
        public void Create_WithInvalidBankAccountNumber_ShouldThrowArgumentException(string invalidBankAccount)
        {
            Action act = () => Customer.Create(
                _validFirstName,
                _validLastName,
                _validDateOfBirth,
                _validEmail,
                _validPhoneNumber,
                invalidBankAccount);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*Bank*account*");
        }

        [Fact]
        public void Update_WithValidData_ShouldUpdateCustomerAndRaiseEvent()
        {
            var customer = Customer.Create(
                _validFirstName,
                _validLastName,
                _validDateOfBirth,
                _validEmail,
                _validPhoneNumber,
                _validBankAccountNumber);
            
            customer.ClearDomainEvents();
            
            var newEmail = Email.Create("updated@hotmail.com");
            var newPhoneNumber = PhoneNumber.Create("+9876543210");

            customer.Update(
                "Updatedsoheil",
                "Updatedkm",
                _validDateOfBirth,
                newEmail,
                newPhoneNumber,
                "98765432109876543210");

            customer.FirstName.Should().Be("Updatedsoheil");
            customer.LastName.Should().Be("Updatedkm");
            customer.Email.Should().Be(newEmail);
            customer.PhoneNumber.Should().Be(newPhoneNumber);
            customer.BankAccountNumber.Should().Be("98765432109876543210");
            
            customer.DomainEvents.Should().HaveCount(1);
            customer.DomainEvents.First().Should().BeOfType<CustomerUpdatedEvent>();
            var updatedEvent = (CustomerUpdatedEvent)customer.DomainEvents.First();
            updatedEvent.CustomerId.Should().Be(customer.Id);
        }

        [Fact]
        public void Delete_ShouldRaiseEvent()
        {
            var customer = Customer.Create(
                _validFirstName,
                _validLastName,
                _validDateOfBirth,
                _validEmail,
                _validPhoneNumber,
                _validBankAccountNumber);
            
            customer.ClearDomainEvents();

            customer.Delete();
            
            customer.DomainEvents.Should().HaveCount(1);
            customer.DomainEvents.First().Should().BeOfType<CustomerDeletedEvent>();
            var deletedEvent = (CustomerDeletedEvent)customer.DomainEvents.First();
            deletedEvent.CustomerId.Should().Be(customer.Id);
        }

        [Fact]
        public void GetUniquenessKey_ShouldReturnCorrectKey()
        {
            var customer = Customer.Create(
                _validFirstName,
                _validLastName,
                _validDateOfBirth,
                _validEmail,
                _validPhoneNumber,
                _validBankAccountNumber);

            var uniquenessKey = customer.UniquenessKey;

            var expectedKey = $"{_validFirstName.ToLowerInvariant()}|{_validLastName.ToLowerInvariant()}|{_validDateOfBirth:yyyy-MM-dd}";
            uniquenessKey.Should().Be(expectedKey);
        }

        [Fact]
        public void Create_WithNullEmail_ShouldThrowArgumentNullException()
        {
            Action act = () => Customer.Create(
                _validFirstName,
                _validLastName,
                _validDateOfBirth,
                null,
                _validPhoneNumber,
                _validBankAccountNumber);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Create_WithNullPhoneNumber_ShouldThrowArgumentNullException()
        {
            Action act = () => Customer.Create(
                _validFirstName,
                _validLastName,
                _validDateOfBirth,
                _validEmail,
                null,
                _validBankAccountNumber);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Age_ShouldCalculateCorrectAge()
        {
            var birthDate = new DateTime(1990, 6, 15);
            var customer = Customer.Create(
                _validFirstName,
                _validLastName,
                birthDate,
                _validEmail,
                _validPhoneNumber,
                _validBankAccountNumber);

            var age = customer.Age;

            var expectedAge = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now < birthDate.AddYears(expectedAge))
                expectedAge--;
                
            age.Should().Be(expectedAge);
        }
    }
}