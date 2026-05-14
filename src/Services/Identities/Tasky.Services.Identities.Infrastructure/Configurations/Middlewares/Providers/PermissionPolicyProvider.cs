using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Tasky.Services.Identities.Infrastructure.Configurations.Middlewares.Handlers;

namespace Tasky.Services.Identities.Infrastructure.Configurations.Middlewares.Providers
{
    public class PermissionPolicyProvider(IOptions<AuthorizationOptions> options):DefaultAuthorizationPolicyProvider(options)
    {
        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);
            if (policy is not null)
            return policy;

            return new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(policyName)).Build();
        }
    }
}