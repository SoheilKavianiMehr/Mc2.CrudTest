using System;
using System.Text.RegularExpressions;

namespace Mc2.CrudTest.Domain.Customers.ValueObjects;

public class PhoneNumber : IEquatable<PhoneNumber>
{
    private static readonly Regex MobileRegex = new(
        @"^\+(\d[\d\s\-\.\(\)]{6,14}\d)$",
        RegexOptions.Compiled);

    public string Value { get; private set; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static PhoneNumber Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentNullException("Mobile phone number cannot be null or empty.", nameof(phoneNumber));

        var cleanedNumber = Regex.Replace(phoneNumber.Trim(), @"[^\d+]", "");

        if (!MobileRegex.IsMatch(phoneNumber.Trim()))
            throw new ArgumentException("Invalid Mobile phone number format. Only mobile numbers are allowed.", nameof(phoneNumber));

        var digitsOnly = Regex.Replace(cleanedNumber, @"[^\d]", "");
        if (digitsOnly.Length < 7 || digitsOnly.Length > 15)
            throw new ArgumentException("Mobile phone number must be between 7 and 15 digits.", nameof(phoneNumber));

        return new PhoneNumber(cleanedNumber);
    }

    public bool Equals(PhoneNumber? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as PhoneNumber);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(PhoneNumber phoneNumber)
    {
        return phoneNumber.Value;
    }

    public static bool operator ==(PhoneNumber? left, PhoneNumber? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(PhoneNumber? left, PhoneNumber? right)
    {
        return !Equals(left, right);
    }
}