namespace ERP.Domain.Models.Entities.Account.SubLeadgers;

public class Customer : SubLeadgerBaseEntity<Customer>
{
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? TaxNumber { get; set; }
    public string? Address { get; set; }
    public CustomerType? CustomerType { get; set; }
}