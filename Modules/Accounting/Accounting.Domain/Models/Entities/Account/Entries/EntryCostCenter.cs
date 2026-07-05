using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using ERP.Domain.Models.Entities.Account.CostCenters;
using Shared.BaseEntities;
using System.Text.Json.Serialization;

namespace ERP.Domain.Models.Entities.Account.Entries;

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