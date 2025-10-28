using Account.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Domain.Commands.SubLeadgers.Suppliers;

public class SupplierUpdateCommand : BaseSubLeadgerUpdateCommand<Supplier>
{
    public string? Mobile { get; set; }
    public string? TaxNumber { get; set; }
    public CustomerType? CustomerType { get; set; }
}