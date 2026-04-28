using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tasky.Services.Identities.Domain.Entities;

namespace Tasky.Services.Identities.Infrastructure.Persistence.EntitiesConfigurations;

public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens", IdentityDb.DEFAULT_SCHEMA);

        builder.HasKey(s => s.Id).HasName("pk_refresh_tokens_id");
        builder.Property(s => s.Id).HasConversion(
            id=> id.Value,
            value => RefreshTokenId.From(value)
        ).ValueGeneratedNever().HasColumnName("id");
        builder.Property(s => s.Token).IsRequired().HasMaxLength(512).HasColumnName("token");
        builder.Property(s => s.Expires).IsRequired().HasColumnName("expires");
        builder.Property(s => s.CreatedAt).IsRequired().HasColumnName("created_at");
        builder.Property(s => s.UserId).IsRequired().HasColumnName("user_id");
        builder.Property(s => s.IsRevoked).IsRequired().HasColumnName("is_revoked");

        builder.Ignore(r => r.IsExpired);
        builder.Ignore(r => r.IsActive);
        builder.Ignore(r => r.RawToken);

        builder.HasIndex(s => s.Token).IsUnique().HasDatabaseName("ux_refresh_tokens_token");
        builder.HasIndex(s => s.UserId).HasDatabaseName("ix_refresh_tokens_user_id");
    }
}