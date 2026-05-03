using Tasky.Services.Identities.Domain.Exceptions;

namespace Tasky.Services.Identities.Domain.Entities;

public readonly record struct PermissionId(Guid Value)
{
    public static PermissionId NewId() => new(Guid.NewGuid());
    public static PermissionId From(Guid value) => 
    value == Guid.Empty ? throw new DomainException("Value cannot be empty.") : new (value);
}


