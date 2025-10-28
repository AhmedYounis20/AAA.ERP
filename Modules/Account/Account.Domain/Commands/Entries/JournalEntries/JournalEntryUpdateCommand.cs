using Account.Domain.Models.Entities.Entries;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;
using Shared.Domain.DTOs.Attachments;

namespace Account.Domain.Commands.Entries.JournalEntries;

public class JournalEntryUpdateCommand : BaseUpdateCommand<Entry>
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