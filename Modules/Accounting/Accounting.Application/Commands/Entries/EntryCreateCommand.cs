using Accounting.Application.Commands.Common;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using Shared.DTOs;
using Accounting.Domain.Models.Entities.Entries;
using System.Text.Json.Serialization;

namespace Accounting.Application.Commands.Entries;

public class EntryCreateCommand : BaseCreateCommand<Entry>, IEntryHeaderCommand
{
    [JsonIgnore]
    public EntryType Type { get; set; } = EntryType.Combined;
    public string EntryNumber { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; }
    public Guid? CurrencyId { get; set; }
    public decimal ExchangeRate { get; set; }
    public string? ReceiverName { get; set; }
    public Guid BranchId { get; set; }
    public DateTime EntryDate { get; set; }
    public Guid FinancialPeriodId { get; set; }
    public Guid? SourceDocumentId { get; set; }
    public string? SourceDocumentType { get; set; }
    public List<AttachmentDto> Attachments { get; set; } = [];
    public List<FinancialTransaction> FinancialTransactions { get; set; } = [];
    public List<EntryCostCenter> CostCenters { get; set; } = [];
}