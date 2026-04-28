using System.Text.RegularExpressions;
using Tasky.Services.Identities.Domain.Exceptions;

namespace Tasky.Services.Identities.Domain.ValueObjects;

public class Email(string  value) : IEquatable<Email>
{
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    public string Value { get; private set; } =value;

    public static Email Create(string email)
    {
        if(string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be empty");
        if(!EmailRegex.IsMatch(email.ToLowerInvariant()))
            throw new DomainException("Email is not valid");
        return new Email(email);
    }

    public bool Equals(Email? other) => other is not null && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj)=> obj is Email other && Equals(other);
    public override string ToString() => Value;
    public override int GetHashCode()=> Value.GetHashCode();
}