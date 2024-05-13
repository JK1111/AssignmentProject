namespace Assignment.Domain.Entities;

public class City : BaseAuditableEntity
{
    public int CountryId { get; set; }

    public string? Name { get; set; }
}
