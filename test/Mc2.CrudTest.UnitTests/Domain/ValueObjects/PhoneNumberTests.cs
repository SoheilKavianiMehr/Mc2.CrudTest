using FluentAssertions;
using Mc2.CrudTest.Domain.ValueObjects;

namespace Mc2.CrudTest.UnitTests.Domain.ValueObjects
{
    public class PhoneNumberTests
    {
        [Theory]
        [InlineData("+1234567890")]  // US format
        [InlineData("+447911123456")] // UK mobile
        [InlineData("+33612345678")] // French mobile
        [InlineData("+491701234567")] // German mobile
        [InlineData("+8613812345678")] // Chinese mobile
        [InlineData("+919876543210")] // Indian mobile
        [InlineData("+5511987654321")] // Brazilian mobile
        public void Create_WithValidMobileNumber_ShouldReturnPhoneNumberInstance(string validMobile)
        {
            // Act
            var phoneNumber = PhoneNumber.Create(validMobile);

            // Assert
            phoneNumber.Should().NotBeNull();
            phoneNumber.Value.Should().Be(validMobile);
        }

        [Theory]
        [InlineData("")]  // Empty
        [InlineData(" ")] // Whitespace
        [InlineData("123")] // Too short
        [InlineData("1234567890")] // Missing country code
        [InlineData("+1234")] // Too short with country code
        [InlineData("+12345678901234567890")] // Too long
        [InlineData("+1-800-555-1234")] // Landline format
        [InlineData("+44207946123")] // UK landline
        [InlineData("+33142345678")] // French landline
        [InlineData("abc123def456")] // Contains letters
        [InlineData("+1 (555) 123-4567")] // Formatted with spaces and brackets
        [InlineData("555-123-4567")] // US format without country code
        public void Create_WithInvalidPhoneNumber_ShouldThrowArgumentException(string invalidPhone)
        {
            // Act & Assert
            Action act = () => PhoneNumber.Create(invalidPhone);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*phone*mobile*");
        }

        [Fact]
        public void Create_WithNullPhoneNumber_ShouldThrowArgumentException()
        {
            // Act & Assert
            Action act = () => PhoneNumber.Create(null);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Equals_WithSamePhoneNumberValue_ShouldReturnTrue()
        {
            // Arrange
            var phone1 = PhoneNumber.Create("+1234567890");
            var phone2 = PhoneNumber.Create("+1234567890");

            // Act & Assert
            phone1.Should().Be(phone2);
            (phone1 == phone2).Should().BeTrue();
            (phone1 != phone2).Should().BeFalse();
        }

        [Fact]
        public void Equals_WithDifferentPhoneNumberValue_ShouldReturnFalse()
        {
            // Arrange
            var phone1 = PhoneNumber.Create("+1234567890");
            var phone2 = PhoneNumber.Create("+9876543210");

            // Act & Assert
            phone1.Should().NotBe(phone2);
            (phone1 == phone2).Should().BeFalse();
            (phone1 != phone2).Should().BeTrue();
        }

        [Fact]
        public void GetHashCode_WithSamePhoneNumberValue_ShouldReturnSameHashCode()
        {
            // Arrange
            var phone1 = PhoneNumber.Create("+1234567890");
            var phone2 = PhoneNumber.Create("+1234567890");

            // Act & Assert
            phone1.GetHashCode().Should().Be(phone2.GetHashCode());
        }

        [Fact]
        public void ImplicitConversion_ToStringFromPhoneNumber_ShouldReturnPhoneNumberValue()
        {
            // Arrange
            var phoneNumber = PhoneNumber.Create("+1234567890");

            // Act
            string phoneString = phoneNumber;

            // Assert
            phoneString.Should().Be("+1234567890");
        }

        [Fact]
        public void ToString_ShouldReturnPhoneNumberValue()
        {
            // Arrange
            var phoneNumber = PhoneNumber.Create("+1234567890");

            // Act
            var result = phoneNumber.ToString();

            // Assert
            result.Should().Be("+1234567890");
        }

        [Theory]
        [InlineData("+1800555123")] // US toll-free (should be rejected as not mobile)
        [InlineData("+44800123456")] // UK toll-free
        [InlineData("+33800123456")] // French toll-free
        public void Create_WithTollFreeNumbers_ShouldThrowArgumentException(string tollFreeNumber)
        {
            // Act & Assert
            Action act = () => PhoneNumber.Create(tollFreeNumber);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*mobile*");
        }
    }
}