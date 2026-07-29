using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tasky.Services.Projects.Domain.Entities;

namespace Tasky.Services.Projects.Infrastructure.Persistence.EntityConfiigurations;

public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(x => x.Id).HasName("pk_categories_id");
        builder.Property(x => x.Id).HasConversion(
            id => id.Value,
            value => CategoryId.From(value)
        ).ValueGeneratedNever().HasColumnName("id");
        builder.HasIndex(x => x.CategoryName).IsUnique();
        builder.Property(x => x.CategoryName).IsRequired().HasMaxLength(250);
        builder.HasMany(x => x.Projects)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.ToTable("categories", ProjectDb.DEFAULT_SCHEMA);
        
    }
}
