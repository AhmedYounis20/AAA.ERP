using ERP.Domain.Commands.Account.SubLeadgers.BaseSubLeadgersCommands;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Domain.Commands.Account.SubLeadgers.Taxes;

public class TaxCreateCommand : BaseSubLeadgerCreateCommand<Tax>
{
    public decimal Percent { get; set; }
    public Guid ChartOfAccountId { get; set; }
}
