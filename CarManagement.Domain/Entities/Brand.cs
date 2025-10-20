using CarManagement.Domain.Enums;

namespace CarManagement.Domain.Entities;

public class Brand : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int FoundedYear { get; set; }

    public ICollection<Line> Lines { get; set; } = new List<Line>();
}
