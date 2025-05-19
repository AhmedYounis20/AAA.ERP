using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Models.Dtos.Attachments;
using ERP.Domain.Models.Entities.Account.Entries;

namespace ERP.Domain.Commands.Account.Entries.JournalEntries;

public class JournalEntryCreateCommand : BaseCreateCommand<Entry>
{
    public string EntryNumber { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; }
    public Guid? CurrencyId { get; set; }
    public decimal ExchangeRate { get; set; }
    public string? ReceiverName { get; set; }
    public Guid BranchId { get; set; }
    public DateTime EntryDate { get; set; }
    public string? Notes { get; set; }
    public Guid FinancialPeriodId { get; set; }
    public List<AttachmentDto> Attachments { get; set; }
    public List<FinancialTransaction> FinancialTransactions { get; set; }
}