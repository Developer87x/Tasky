using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tasky.Services.Projects.Domain.Entities;

namespace Tasky.Services.Projects.Infrastructure.Persistence.EntityConfiigurations;

public class ProjectEntityConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(x => x.Id).HasName("pk_projects_id");
        builder.Property(x => x.Id).HasConversion(
            id => id.Value,
            value => ProjectId.From(value)
        ).ValueGeneratedNever().HasColumnName("id");
        builder.HasIndex(x => x.ProjectName).IsUnique();
        builder.ToTable("projects", ProjectDb.DEFAULT_SCHEMA);
    }
}