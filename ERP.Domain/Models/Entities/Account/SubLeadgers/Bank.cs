namespace ERP.Domain.Models.Entities.Account.SubLeadgers;

public class Bank : SubLeadgerBaseEntity<Bank>
{
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? BankAccount { get; set; }
    public string? BankAddress { get; set; }
}