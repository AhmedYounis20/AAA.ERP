using Domain.Account.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Domain.Account.Models.Entities.SubLeadgers;

namespace Domain.Account.Commands.SubLeadgers.Customers;

public class CustomerCreateCommand : BaseSubLeadgerCreateCommand<Customer>
{
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? TaxNumber { get; set; }
    public string? Address { get; set; }
    public CustomerType? CustomerType { get; set; }
}