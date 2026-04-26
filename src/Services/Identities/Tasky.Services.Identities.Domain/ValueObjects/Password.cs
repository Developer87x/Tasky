using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tasky.Services.Identities.Domain.Exceptions;

namespace Tasky.Services.Identities.Domain.ValueObjects
{
    public class Password(string value) :IEquatable<Password>
    {

        public static readonly Regex SpeciaclCharactersRegex = new(@"[!@#$%^&*(),.?""':{}|<>]",RegexOptions.Compiled);
        public string Value { get; private set; } = value;

        public static Password FromHash(string hash)
        {
            if(string.IsNullOrWhiteSpace(hash))
                throw new BadRequestException("Password hash cannot be empty");
            return new Password(hash);
        
        }

        public static void Validate(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new BadRequestException("Password cannot be empty");
            if (password.Length < 8)
                throw new BadRequestException("Password must be at least 8 characters long");
            if (!SpeciaclCharactersRegex.IsMatch(password))
                throw new BadRequestException("Password must contain at least one special character");
        }

        public bool Equals(Password? other)
                =>other is not null && Value == other.Value;
    }
}