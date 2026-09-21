using System.Text.RegularExpressions;
using Tasky.BuildingBlocks.Core.Exceptions;
using Tasky.BuildingBlocks.Core.Models;

namespace Tasky.Services.Identities.Domain.ValueObjects;

public class Password(string value) : ValueObject
{
    private static readonly Regex SpecialCharactersRegex = new Regex(@"[!@#$%^&*(),.?""':{}|<>]", RegexOptions.Compiled);
    public string Value { get; } = value;

    public static Password FromHash(string hash) => string.IsNullOrWhiteSpace(hash) ? throw new BadRequestException("Password hash cannot be empty") : new Password(hash);

    public static void Validate(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new BadRequestException("Password cannot be empty");
        if (password.Length < 8)
            throw new BadRequestException("Password must be at least 8 characters long");
        if (!SpecialCharactersRegex.IsMatch(password))
            throw new BadRequestException("Password must contain at least one special character");
    }

    public bool Equals(Password? other)
            =>other is not null && Value == other.Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override bool Equals(object? obj)
        => obj is Password other && Equals(other);
    public override int GetHashCode()=> Value.GetHashCode();
}
