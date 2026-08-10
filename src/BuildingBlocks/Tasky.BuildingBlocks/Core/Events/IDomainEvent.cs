using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasky.BuildingBlocks.Core.Events;

public interface IDomainEvent : IEvent
{
}



public interface IDomainEventHandler<in TDomainEvent>   where TDomainEvent : IDomainEvent
{
    Task HandleAsync(TDomainEvent domainEvent, CancellationToken cancellationToken =default);
}