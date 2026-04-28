using Tasky.Services.Identities.Domain.Exceptions;

namespace Tasky.Services.Identities.Domain.Entities;

public readonly record struct UserId(Guid Value)
{
    public static UserId NewId() => new(Guid.NewGuid());
    public static UserId From(Guid value) => 
    value == Guid.Empty ? throw new DomainException("Value cannot be empty.") : new (value);
}


