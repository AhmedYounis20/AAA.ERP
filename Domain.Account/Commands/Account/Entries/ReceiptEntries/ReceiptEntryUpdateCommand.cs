using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Domain.Account.Models.Dtos.Attachments;
using Domain.Account.Models.Dtos.Entry;
using ERP.Domain.Models.Entities.Account.Entries;

namespace ERP.Domain.Commands.Account.Entries.ReceiptEntries;

public class ReceiptEntryUpdateCommand : BaseUpdateCommand<Entry>
{
    public string EntryNumber { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; }
    public Guid? CurrencyId { get; set; }
    public decimal ExchangeRate { get; set; }
    public string? ReceiverName { get; set; }
    public Guid BranchId { get; set; }
    public DateTime EntryDate { get; set; }
    public Guid FinancialPeriodId { get; set; }
    public List<AttachmentDto> Attachments { get; set; } = [];
    public List<ComplexFinancialTransactionDto> FinancialTransactions { get; set; } = [];
    public List<EntryCostCenter> CostCenters { get; set; } = [];
}