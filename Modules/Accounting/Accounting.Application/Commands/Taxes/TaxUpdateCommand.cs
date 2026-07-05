using Accounting.Domain.Entities;
using Accounting.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;

namespace Accounting.Application.Commands.Taxes;

public class TaxUpdateCommand : BaseSubLeadgerUpdateCommand<Tax>
{
    public decimal Percent { get; set; }
}
