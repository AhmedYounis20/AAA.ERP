using ERP.Domain.Models.Entities.Account.ChartOfAccounts;

namespace ERP.Domain.Models.Entities.Account.SubLeadgers;

public class FixedAsset : SubLeadgerBaseEntity<FixedAsset>
{
    public Guid? ExpensesAccountId { get; set; }
    public ChartOfAccount? ExpensesAccount { get; set; }
    public Guid? AccumlatedAccountId { get; set; }
    public ChartOfAccount? AccumlatedAccount { get; set; }
    public string? Serial { get; set; }
    public FixedAssetType? FixedAssetType { get; set; }
    public string? Model { get; set; }
    public string? Version { get; set; }
    public string? ManufactureCompany { get; set; }
    public bool IsDepreciable { get; set; }
    public int AssetLifeSpanByYears { get; set; }
    public int DepreciationRate { get; set; }
}