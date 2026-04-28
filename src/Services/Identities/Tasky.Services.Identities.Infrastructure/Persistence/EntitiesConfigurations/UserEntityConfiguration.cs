using Microsoft.EntityFrameworkCore;
using Tasky.Services.Identities.Domain.Entities;

namespace Tasky.Services.Identities.Infrastructure.Persistence.EntitiesConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
    {
        builder.HasKey(s => s.Id).HasName("pk_users_id");
        builder.Property(s=>s.Id).HasConversion(
            id => id.Value,
            value => UserId.From(value)
        ).ValueGeneratedNever().HasColumnName("id");
        builder.Property(s => s.UserName).IsRequired().HasMaxLength(100).HasColumnName("username");
        builder.Property(s => s.IsActive).HasColumnName("is_active");
        builder.Property(s => s.CreatedAt).IsRequired().HasColumnName("created_at");
        builder.Property(s => s.UpdatedAt).HasColumnName("updated_at");
        builder.ToTable("users", IdentityDb.DEFAULT_SCHEMA);

        // configure the relationship between User and Role
        builder.HasMany(s => s.Roles)
            .WithMany(r => r.Users).UsingEntity<Dictionary<string, object>>(
                "user_roles",
                j => j.HasOne<Role>().WithMany().HasForeignKey("role_id").HasConstraintName("fk_userroles_roleid").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<User>().WithMany().HasForeignKey("user_id").HasConstraintName("fk_userroles_userid").OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("user_id", "role_id").HasName("pk_userroles_userid_roleid");
                    j.ToTable("user_roles", IdentityDb.DEFAULT_SCHEMA);
                }
            );
        builder.HasIndex(s => s.UserName).IsUnique().HasDatabaseName("ux_users_username");
        builder.Navigation(s => s.Roles).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(s => s.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .HasConstraintName("fk_refreshtokens_userid")
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(s => s.RefreshTokens).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.OwnsOne(s => s.Password, p =>
        {
            p.Property(p => p.Value).IsRequired().HasColumnName("password").HasMaxLength(300);
        });
        builder.OwnsOne(s => s.Email, e =>
        {
            e.Property(e => e.Value).IsRequired().HasColumnName("email").HasMaxLength(255);
            e.HasIndex(e => e.Value).IsUnique().HasDatabaseName("ux_users_email");
        });
        builder.Ignore(s=>s.DomainEvents);
    }
}
