using Accounting.Domain.Entities;
using Accounting.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;

namespace Accounting.Application.Commands.Taxes;

public class TaxCreateCommand : BaseSubLeadgerCreateCommand<Tax>
{
    public decimal Percent { get; set; }
    public Guid ChartOfAccountId { get; set; }
}
