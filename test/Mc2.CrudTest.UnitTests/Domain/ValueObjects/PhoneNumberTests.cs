using FluentAssertions;
using Mc2.CrudTest.Domain.Customers.ValueObjects;

namespace Mc2.CrudTest.UnitTests.Domain.ValueObjects
{
    public class PhoneNumberTests
    {
        [Theory]
        [InlineData("+1234567890")]
        [InlineData("+447911123456")]
        [InlineData("+33612345678")] 
        [InlineData("+491701234567")]
        [InlineData("+8613812345678")]
        [InlineData("+919876543210")]
        [InlineData("+5511987654321")] 
        public void Create_WithValidMobileNumber_ShouldReturnPhoneNumberInstance(string validMobile)
        {
            var phoneNumber = PhoneNumber.Create(validMobile);

            phoneNumber.Should().NotBeNull();
            phoneNumber.Value.Should().Be(validMobile);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("123")]
        [InlineData("1234567890")]
        [InlineData("+1234")]
        [InlineData("+12345678901234567890")]
        [InlineData("abc123def456")]
        [InlineData("555-123-4567")]
        public void Create_WithInvalidPhoneNumber_ShouldThrowArgumentException(string invalidPhone)
        {
            Action act = () => PhoneNumber.Create(invalidPhone);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*mobile*phone*number*");
        }

        [Fact]
        public void Create_WithNullPhoneNumber_ShouldThrowArgumentException()
        {
            Action act = () => PhoneNumber.Create(null);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Equals_WithSamePhoneNumberValue_ShouldReturnTrue()
        {
            var phone1 = PhoneNumber.Create("+1234567890");
            var phone2 = PhoneNumber.Create("+1234567890");

            phone1.Should().Be(phone2);
            (phone1 == phone2).Should().BeTrue();
            (phone1 != phone2).Should().BeFalse();
        }

        [Fact]
        public void Equals_WithDifferentPhoneNumberValue_ShouldReturnFalse()
        {
            var phone1 = PhoneNumber.Create("+1234567890");
            var phone2 = PhoneNumber.Create("+9876543210");

            phone1.Should().NotBe(phone2);
            (phone1 == phone2).Should().BeFalse();
            (phone1 != phone2).Should().BeTrue();
        }

        [Fact]
        public void GetHashCode_WithSamePhoneNumberValue_ShouldReturnSameHashCode()
        {
            var phone1 = PhoneNumber.Create("+1234567890");
            var phone2 = PhoneNumber.Create("+1234567890");

            phone1.GetHashCode().Should().Be(phone2.GetHashCode());
        }

        [Fact]
        public void ImplicitConversion_ToStringFromPhoneNumber_ShouldReturnPhoneNumberValue()
        {
            var phoneNumber = PhoneNumber.Create("+1234567890");

            string phoneString = phoneNumber;

            phoneString.Should().Be("+1234567890");
        }

        [Fact]
        public void ToString_ShouldReturnPhoneNumberValue()
        {
            var phoneNumber = PhoneNumber.Create("+1234567890");

            var result = phoneNumber.ToString();

            result.Should().Be("+1234567890");
        }
    }
}