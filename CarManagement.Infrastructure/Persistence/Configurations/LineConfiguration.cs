using CarManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarManagement.Infrastructure.Persistence.Configurations;

public class LineConfiguration : IEntityTypeConfiguration<Line>
{
    public void Configure(EntityTypeBuilder<Line> builder)
    {
        builder.ToTable("Lines");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Category).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
        builder.HasIndex(x => new { x.BrandId, x.Name }).IsUnique();
        builder.HasIndex(x => new { x.Category, x.IsDeleted });
        builder.HasOne(x => x.Brand).WithMany(x => x.Lines).HasForeignKey(x => x.BrandId);
        builder.HasMany(x => x.ModelYears).WithOne(x => x.Line!).HasForeignKey(x => x.LineId);
    }
}
