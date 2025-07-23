using FluentValidation;
using PhoneNumbers;
using System.Text.RegularExpressions;

namespace Mc2.CrudTest.Application.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    private readonly PhoneNumberUtil _phoneNumberUtil;

    public UpdateCustomerCommandValidator()
    {
        _phoneNumberUtil = PhoneNumberUtil.GetInstance();

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .Must(BeValidAge).WithMessage("Customer must be at least 18 years old.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Must(BeValidMobileNumber).WithMessage("Phone number must be a valid mobile number.");

        RuleFor(x => x.BankAccountNumber)
            .NotEmpty().WithMessage("Bank account number is required.")
            .Must(BeValidBankAccountNumber).WithMessage("Bank account number must be valid.");
    }

    private bool BeValidAge(DateTime dateOfBirth)
    {
        var age = DateTime.Today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > DateTime.Today.AddYears(-age))
            age--;
        return age >= 18;
    }

    private bool BeValidMobileNumber(string phoneNumber)
    {
        try
        {
            var parsedNumber = _phoneNumberUtil.Parse(phoneNumber, null);

            if (!_phoneNumberUtil.IsValidNumber(parsedNumber))
                return false;

            var numberType = _phoneNumberUtil.GetNumberType(parsedNumber);
            return numberType == PhoneNumberType.MOBILE || numberType == PhoneNumberType.FIXED_LINE_OR_MOBILE;
        }
        catch
        {
            return false;
        }
    }

    private bool BeValidBankAccountNumber(string bankAccountNumber)
    {
        var cleanNumber = Regex.Replace(bankAccountNumber, @"[\s-]", "");

        if (!Regex.IsMatch(cleanNumber, @"^\d+$"))
            return false;

        if (cleanNumber.Length < 8 || cleanNumber.Length > 20)
            return false;

        return true;
    }
}