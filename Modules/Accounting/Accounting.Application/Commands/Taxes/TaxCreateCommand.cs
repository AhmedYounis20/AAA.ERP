using Accounting.Application.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Accounting.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Application.Commands.Taxes;

public class TaxCreateCommand : BaseSubLeadgerCreateCommand<Tax>
{
    public decimal Percent { get; set; }
    public Guid ChartOfAccountId { get; set; }
}
