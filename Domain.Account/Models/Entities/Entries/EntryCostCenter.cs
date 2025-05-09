using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.CostCenters;
using Shared.BaseEntities;

namespace Domain.Account.Models.Entities.Entries;

public class EntryCostCenter : BaseEntity
{
    public Guid EntryId { get; set; }
    public Entry? Entry { get; set; }
    public Guid CostCenterId { get; set; }
    public CostCenter? CostCenter { get; set; }
    public Decimal Amount { get; set; }
    public AccountNature AccountNature { get; set; } = AccountNature.Debit;
}