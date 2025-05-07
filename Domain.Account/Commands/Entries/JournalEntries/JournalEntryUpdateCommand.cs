using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Domain.Account.Models.Dtos.Attachments;
using Domain.Account.Models.Entities.Entries;

namespace Domain.Account.Commands.Entries.JournalEntries;

public class JournalEntryUpdateCommand : BaseUpdateCommand<Entry>
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
    public List<AttachmentDto> Attachments { get; set; }
    public List<FinancialTransaction> FinancialTransactions { get; set; }
}