using Account.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Domain.Commands.SubLeadgers.Customers;

public class CustomerCreateCommand : BaseSubLeadgerCreateCommand<Customer>
{
    public string? Mobile { get; set; }
    public string? TaxNumber { get; set; }
    public CustomerType? CustomerType { get; set; }
}