using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Models.Entities.Entries;
using Shared.Responses;

namespace Domain.Account.Commands.Currencies;

public class EntryUpdateCommand : BaseUpdateCommand<Entry>
{
    public string EntryNumber { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; }
    public Guid? CurrencyId { get; set; }
    public decimal ExchageRate { get; set; }
    public string? ReceiverName { get; set; }
    public Guid BranchId { get; set; }
    public DateTime EntryDate { get; set; }
    public string? Notes { get; set; }
    public Guid FinancialPeriodId { get; set; }
}