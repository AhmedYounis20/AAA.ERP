using Accounting.Domain.Models.Entities.Currencies;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Shared.BaseEntities;

namespace Accounting.Domain.Models.Entities.Entries;

public class Entry : BaseEntity
{
    public EntryType EntryType { get; set; } = EntryType.Combined;
    public string EntryNumber { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; }
    public Guid? CurrencyId { get; set; }
    public virtual Currency? Currency { get; set; }
    public decimal ExchangeRate { get; set; }
    public string? ReceiverName { get; set; }
    public Guid BranchId { get; set; }
    public Branch? Branch { get; set; }
    public DateTime EntryDate { get; set; }
    public string? Notes { get; set; }
    public Guid FinancialPeriodId { get; set; }
    public virtual FinancialPeriod? FinancialPeriod { get; set; }

    /// <summary>
    /// Originating document id for idempotent cross-module posting (e.g. inventory transaction).
    /// </summary>
    public Guid? SourceDocumentId { get; set; }

    /// <summary>
    /// Discriminator for <see cref="SourceDocumentId"/> — see <c>EntrySourceDocumentTypes</c>.
    /// </summary>
    public string? SourceDocumentType { get; set; }

    public virtual List<EntryAttachment> EntryAttachments { get; set; } = [];
    public virtual List<FinancialTransaction> FinancialTransactions { get; set; } = [];
    public virtual List<EntryCostCenter> CostCenters { get; set; } = [];
}