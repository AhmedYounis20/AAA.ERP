using Account.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Domain.Commands.SubLeadgers.FixedAssets;

public class FixedAssetUpdateCommand : BaseSubLeadgerUpdateCommand<FixedAsset>
{
    public string? Serial { get; set; }
    public string? Model { get; set; }
    public string? Version { get; set; }
    public string? ManufactureCompany { get; set; }
    public bool IsDepreciable { get; set; }
    public int AssetLifeSpanByYears { get; set; }
    public int DepreciationRate { get; set; }
}