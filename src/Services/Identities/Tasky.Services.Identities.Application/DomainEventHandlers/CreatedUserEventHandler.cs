using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Domain.DomainEvents;
using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Application.bin.DomainEventHandlers
{
    public class CreatedUserEventHandler(ILogger<CreatedUserEventHandler> logger) : IDomainEventHandler<CreatedUserDomainEvent>
    {
        private readonly ILogger<CreatedUserEventHandler> _logger= logger;
        public Task Handle(CreatedUserDomainEvent domainEvent)
        {
            _logger.LogInformation("User with id {UserId} was created at {DateOccurred}", domainEvent.Id, domainEvent.DateOccurred);
            return Task.CompletedTask;
        }
    }
}