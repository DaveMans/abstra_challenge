namespace CarManagement.Domain.Entities;

public class ModelYear : BaseEntity
{
    public Guid LineId { get; set; }
    public int Year { get; set; }
    public decimal BasePrice { get; set; }
    public string Features { get; set; } = "{}";

    public Line? Line { get; set; }
}
