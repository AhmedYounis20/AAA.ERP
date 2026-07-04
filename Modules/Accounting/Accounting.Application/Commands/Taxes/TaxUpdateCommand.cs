using Accounting.Domain.Entities;
using ERP.Domain.Commands.Account.SubLeadgers.BaseSubLeadgersCommands;

namespace Accounting.Application.Commands.Taxes;

public class TaxUpdateCommand : BaseSubLeadgerUpdateCommand<Tax>
{
    public decimal Percent { get; set; }
}
