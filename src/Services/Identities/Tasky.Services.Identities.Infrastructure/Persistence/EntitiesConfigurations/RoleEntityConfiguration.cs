using Microsoft.EntityFrameworkCore;
using Tasky.Services.Identities.Domain.Entities;

namespace Tasky.Services.Identities.Infrastructure.Persistence.EntitiesConfigurations;

public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(s => s.Id).HasName("pk_roles_id");
        builder.Property(s => s.Id).HasConversion(
            id => id.Value,
            value => RoleId.From(value)
        ).ValueGeneratedNever().HasColumnName("id");

        builder.HasMany(s => s.Permissions)
            .WithMany(p => p.Roles).UsingEntity<Dictionary<string, object>>(
                "role_permissions",
                j => j.HasOne<Permission>().WithMany().HasForeignKey("permission_id").HasConstraintName("fk_rolepermissions_permissionid").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Role>().WithMany().HasForeignKey("role_id").HasConstraintName("fk_rolepermissions_roleid").OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("role_id", "permission_id").HasName("pk_rolepermissions_roleid_permissionid");
                    j.ToTable("role_permissions", IdentityDb.DEFAULT_SCHEMA);
                }
            );

        builder.Property(s => s.RoleName).IsRequired().HasMaxLength(100).HasColumnName("role_name");
        builder.ToTable("roles", IdentityDb.DEFAULT_SCHEMA);
        builder.Navigation(s => s.Users).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

public class PermissionEntityConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(s => s.Id).HasName("pk_permissions_id");
        builder.Property(s => s.Id).HasConversion(
            id => id.Value,
            value => PermissionId.From(value)
        ).ValueGeneratedNever().HasColumnName("id");

        builder.Property(s => s.PermissionName).IsRequired().HasMaxLength(100).HasColumnName("permission_name");
        builder.ToTable("permissions", IdentityDb.DEFAULT_SCHEMA);
        builder.Navigation(s => s.Roles).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}