using Accounting.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Commands.Customers;

public class CustomerUpdateCommand : BaseSubLeadgerUpdateCommand<Customer>
{
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? TaxNumber { get; set; }
    public string? Address { get; set; }
    public CustomerType? CustomerType { get; set; }
}
