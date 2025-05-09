using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Models.Entities.SubLeadgers;
using Shared.BaseEntities;

namespace Domain.Account.Models.Entities.Entries;

public class Entry : BaseEntity
{
    public EntryType EntryType { get; set; } = EntryType.Compined;
    public string EntryNumber { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; }
    public Guid? CurrencyId { get; set; }
    public virtual Currency? Currency { get; set; }
    public decimal ExchageRate { get; set; }
    public string? ReceiverName { get; set; }
    public Guid BranchId { get; set; }
    public Branch? Branch { get; set; }
    public DateTime EntryDate { get; set; }
    public string? Notes { get; set; }
    public Guid FinancialPeriodId { get; set; }
    public virtual FinancialPeriod? FinancialPeriod { get; set; }
    public virtual List<EntryAttachment> EntryAttachments { get; set; } = [];
    public virtual List<FinancialTransaction> FinancialTransactions { get; set; } = [];
    public virtual List<EntryCostCenter> CostCenters { get; set; } = [];
}