using Accounting.Application.Commands.Common;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Application.DTOs;
using Accounting.Application.Dtos.Entries;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Application.Commands.ReceiptEntries;

public class ReceiptEntryUpdateCommand : BaseUpdateCommand<Entry>, IEntryHeaderCommand
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
