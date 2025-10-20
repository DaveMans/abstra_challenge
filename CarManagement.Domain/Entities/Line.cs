using CarManagement.Domain.Enums;

namespace CarManagement.Domain.Entities;

public class Line : BaseEntity
{
    public Guid BrandId { get; set; }
    public string Name { get; set; } = string.Empty;
    public CarCategory Category { get; set; }

    public Brand? Brand { get; set; }
    public ICollection<ModelYear> ModelYears { get; set; } = new List<ModelYear>();
}
