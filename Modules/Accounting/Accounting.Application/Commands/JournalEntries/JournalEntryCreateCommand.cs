using Accounting.Application.Commands.Common;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using Shared.DTOs;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Application.Commands.JournalEntries;

public class JournalEntryCreateCommand : BaseCreateCommand<Entry>, IEntryHeaderCommand
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
