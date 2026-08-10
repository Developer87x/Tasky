using Microsoft.Extensions.DependencyInjection;
using Tasky.Services.Projects.Domain.Repositories;
using Tasky.Services.Projects.Infrastructure.Persistence.Repositories;

namespace Tasky.Services.Projects.Infrastructure.Configurations.Extensions;

public static class AddRepositoriesExtension
{
    extension(IServiceCollection service)
    {
        public IServiceCollection AddRepositories()
        {
            service.AddScoped<IProjectRepository, ProjectRepository>();
            service.AddScoped<ICategoryRepository, CategoryRepository>();
            return service;
        }
    }
}