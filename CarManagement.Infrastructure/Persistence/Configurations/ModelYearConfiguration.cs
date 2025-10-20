using CarManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarManagement.Infrastructure.Persistence.Configurations;

public class ModelYearConfiguration : IEntityTypeConfiguration<ModelYear>
{
    public void Configure(EntityTypeBuilder<ModelYear> builder)
    {
        builder.ToTable("ModelYears");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Year).IsRequired();
        builder.Property(x => x.BasePrice).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Features).HasColumnType("nvarchar(max)").IsRequired();
        builder.HasIndex(x => new { x.LineId, x.Year }).IsUnique();
    }
}
