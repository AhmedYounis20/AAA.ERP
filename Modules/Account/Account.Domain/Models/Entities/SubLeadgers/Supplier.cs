namespace Account.Domain.Models.Entities.SubLeadgers;

public class Supplier : SubLeadgerBaseEntity<Supplier>
{
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? TaxNumber { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
    public CustomerType? CustomerType { get; set; }
}
