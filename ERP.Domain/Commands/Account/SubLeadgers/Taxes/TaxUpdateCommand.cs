using ERP.Domain.Commands.Account.SubLeadgers.BaseSubLeadgersCommands;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Domain.Commands.Account.SubLeadgers.Taxes;

public class TaxUpdateCommand : BaseSubLeadgerUpdateCommand<Tax>
{
    public decimal Percent { get; set; }
}
