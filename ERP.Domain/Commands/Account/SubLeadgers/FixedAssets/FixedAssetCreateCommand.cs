using ERP.Domain.Commands.Account.SubLeadgers.BaseSubLeadgersCommands;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Domain.Commands.Account.SubLeadgers.FixedAssets;

public class FixedAssetCreateCommand : BaseSubLeadgerCreateCommand<FixedAsset>
{
    public string? Serial { get; set; }
    public string? Model { get; set; }
    public string? Version { get; set; }
    public string? AccumelatedCode { get; set; }
    public string? ExpensesCode { get; set; }
    public string? ManufactureCompany { get; set; }
    public bool IsDepreciable { get; set; }
    public int AssetLifeSpanByYears { get; set; }
    public int DepreciationRate { get; set; }
    public FixedAssetType? FixedAssetType { get; set; }

}