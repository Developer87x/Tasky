using Microsoft.EntityFrameworkCore;
using Tasky.Services.Identities.Domain.Entities;

namespace Tasky.Services.Identities.Infrastructure.Persistence.EntitiesConfigurations;

public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(s => s.Id).HasName("pk_roles_id");
        builder.Property(s => s.Id).ValueGeneratedNever().HasColumnName("id");
        builder.Property(s => s.RoleName).IsRequired().HasMaxLength(100).HasColumnName("role_name");
        builder.ToTable("roles", IdentityDb.DEFAULT_SCHEMA);
        builder.Navigation(s => s.Users).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}