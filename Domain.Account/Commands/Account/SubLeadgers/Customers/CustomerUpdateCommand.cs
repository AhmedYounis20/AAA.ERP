using ERP.Domain.Commands.Account.SubLeadgers.BaseSubLeadgersCommands;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Domain.Commands.Account.SubLeadgers.Customers;

public class CustomerUpdateCommand : BaseSubLeadgerUpdateCommand<Customer>
{
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? TaxNumber { get; set; }
    public string? Address { get; set; }
    public CustomerType? CustomerType { get; set; }
}