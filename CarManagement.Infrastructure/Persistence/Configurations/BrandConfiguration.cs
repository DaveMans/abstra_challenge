using CarManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarManagement.Infrastructure.Persistence.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Country).IsRequired().HasMaxLength(100);
        builder.Property(x => x.FoundedYear).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasMany(x => x.Lines).WithOne(x => x.Brand!).HasForeignKey(x => x.BrandId);
        builder.HasIndex(x => new { x.Country, x.IsDeleted });
        builder.HasIndex(x => new { x.IsActive, x.IsDeleted });
    }
}
