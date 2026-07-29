using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tasky.Services.Projects.Infrastructure.Persistence;

namespace Tasky.Services.Projects.Infrastructure.Configurations.Extensions;

public static class DatabaseExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddProjectDatabase(IConfiguration configuration)
        {
            services.AddDbContext<ProjectDb>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("ProjectDbStr"),
                s =>
                {
                    s.MigrationsAssembly("Tasky.Services.Projects.Infrastructure");
                    s.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    s.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
            });
            return services;
        }
    }
}