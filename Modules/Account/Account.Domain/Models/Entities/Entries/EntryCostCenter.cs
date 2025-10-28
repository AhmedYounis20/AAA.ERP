using Account.Domain.Models.Entities.ChartOfAccounts;
using Account.Domain.Models.Entities.CostCenters;

namespace Account.Domain.Models.Entities.Entries;

public class EntryCostCenter : BaseEntity
{
    public Guid EntryId { get; set; }
    [JsonIgnore]
    public Entry? Entry { get; set; }
    public Guid CostCenterId { get; set; }
    public CostCenter? CostCenter { get; set; }
    public decimal Amount { get; set; }
    public AccountNature AccountNature { get; set; } = AccountNature.Debit;
}
