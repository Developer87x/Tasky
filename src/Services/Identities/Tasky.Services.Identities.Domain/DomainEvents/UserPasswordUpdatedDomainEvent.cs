using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Domain.DomainEvents;

public class UserPasswordUpdatedDomainEvent(UserId id) : IDomainEvent
{
    public UserId Id { get; set; } = id;
    public DateTime DateOccurred => DateTime.UtcNow;
    public int Version => throw new NotImplementedException();
}
