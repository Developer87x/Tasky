using Tasky.Services.Identities.Domain.Exceptions;

namespace Tasky.Services.Identities.Domain.Entities;

public readonly record struct RoleId(Guid Value)
{
    public static RoleId NewId() => new(Guid.NewGuid());
    public static RoleId From(Guid value) => 
    value == Guid.Empty ? throw new DomainException("Value cannot be empty.") : new (value);
}


