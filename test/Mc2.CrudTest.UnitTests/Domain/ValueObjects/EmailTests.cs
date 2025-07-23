using FluentAssertions;
using Mc2.CrudTest.Domain.Customers.ValueObjects;

namespace Mc2.CrudTest.UnitTests.Domain.ValueObjects
{
    public class EmailTests
    {
        [Theory]
        [InlineData("test@example.com")]
        [InlineData("firstname+lastname@example.com")]
        [InlineData("1234567890@example.com")]
        [InlineData("email@example-one.com")]
        [InlineData("email@example.name")]
        public void Create_WithValidEmail_ShouldReturnEmailInstance(string validEmail)
        {
            var email = Email.Create(validEmail);

            email.Should().NotBeNull();
            email.Value.Should().Be(validEmail);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("invalid-email")]
        [InlineData("@example.com")]
        [InlineData("test@")]
        [InlineData("test..test@example.com")]
        [InlineData("test@example")]
        [InlineData("test@.com")]
        public void Create_WithInvalidEmail_ShouldThrowArgumentException(string invalidEmail)
        {
            Action act = () => Email.Create(invalidEmail);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*email*");
        }

        [Fact]
        public void Create_WithNullEmail_ShouldThrowArgumentException()
        {
            Action act = () => Email.Create(null);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Equals_WithSameEmailValue_ShouldReturnTrue()
        {
            var email1 = Email.Create("test@example.com");
            var email2 = Email.Create("test@example.com");

            email1.Should().Be(email2);
            (email1 == email2).Should().BeTrue();
            (email1 != email2).Should().BeFalse();
        }

        [Fact]
        public void Equals_WithDifferentEmailValue_ShouldReturnFalse()
        {
            var email1 = Email.Create("test1@example.com");
            var email2 = Email.Create("test2@example.com");

            email1.Should().NotBe(email2);
            (email1 == email2).Should().BeFalse();
            (email1 != email2).Should().BeTrue();
        }

        [Fact]
        public void GetHashCode_WithSameEmailValue_ShouldReturnSameHashCode()
        {
            var email1 = Email.Create("test@example.com");
            var email2 = Email.Create("test@example.com");

            email1.GetHashCode().Should().Be(email2.GetHashCode());
        }

        [Fact]
        public void ImplicitConversion_ToStringFromEmail_ShouldReturnEmailValue()
        {
            var email = Email.Create("test@example.com");

            string emailString = email;

            emailString.Should().Be("test@example.com");
        }

        [Fact]
        public void ToString_ShouldReturnEmailValue()
        {
            var email = Email.Create("test@example.com");

            var result = email.ToString();

            result.Should().Be("test@example.com");
        }
    }
}