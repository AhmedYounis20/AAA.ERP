using Account.Domain.DTOs.Entry;
using Account.Domain.Models.Entities.Entries;
using Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;
using Shared.Domain.DTOs.Attachments;
using System.Text.Json.Serialization;

namespace Account.Domain.Commands.Entries;

public class ComplexEntryCreateCommand : BaseCreateCommand<Entry>
{
    [JsonIgnore]
    public EntryType Type { get; set; } = EntryType.Compined;
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