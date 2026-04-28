using Tasky.Services.Identities.Domain.Exceptions;

namespace Tasky.Services.Identities.Domain.Entities;

public readonly record struct RefreshTokenId(Guid Value)
{
    public static RefreshTokenId NewId() => new(Guid.NewGuid());
    public static RefreshTokenId From(Guid value) => 
    value == Guid.Empty ? throw new DomainException("Value cannot be empty.") : new (value);
}


