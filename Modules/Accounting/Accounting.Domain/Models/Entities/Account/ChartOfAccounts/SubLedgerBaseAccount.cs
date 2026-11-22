using AssetCategory = Accounting.Domain.Models.Entities.SubLeadgers.FixedAssetType;
using ERP.BuildingBlocks.Domain.Entities;

namespace Accounting.Domain.Models.Entities.ChartOfAccounts;

/// <summary>
/// Links one chart-of-account node to the sub-ledger posting role it serves.
/// Chart-of-account rows stay general-ledger nodes; this binding is the configuration.
/// </summary>
public class SubLedgerBaseAccount : BaseEntity
{
    public const string InvalidBindingMessage = "SubLedgerBaseAccountInvalid";

    public Guid ChartOfAccountId { get; set; }
    public ChartOfAccount? ChartOfAccount { get; set; }
    public SubLeadgerType SubLedgerType { get; set; }
    public SubLedgerAccountRole Role { get; set; }
    public AssetCategory? FixedAssetType { get; set; }

    public static SubLedgerBaseAccount Create(
        Guid chartOfAccountId,
        SubLeadgerType subLedgerType,
        SubLedgerAccountRole role,
        AssetCategory? fixedAssetType)
    {
        if (chartOfAccountId == Guid.Empty)
            throw new InvalidOperationException(InvalidBindingMessage);

        EnsureCombination(subLedgerType, role, fixedAssetType);

        return new SubLedgerBaseAccount
        {
            Id = chartOfAccountId,
            ChartOfAccountId = chartOfAccountId,
            SubLedgerType = subLedgerType,
            Role = role,
            FixedAssetType = fixedAssetType
        };
    }

    public static void EnsureCombination(
        SubLeadgerType subLedgerType,
        SubLedgerAccountRole role,
        AssetCategory? fixedAssetType)
    {
        if (!Enum.IsDefined(subLedgerType) || !Enum.IsDefined(role))
            throw new InvalidOperationException(InvalidBindingMessage);

        if (subLedgerType == SubLeadgerType.FixedAsset)
        {
            if (fixedAssetType is null || !Enum.IsDefined(fixedAssetType.Value))
                throw new InvalidOperationException(InvalidBindingMessage);

            if (role is not (
                SubLedgerAccountRole.Asset
                or SubLedgerAccountRole.AccumulatedDepreciation
                or SubLedgerAccountRole.DepreciationExpense))
                throw new InvalidOperationException(InvalidBindingMessage);

            if (fixedAssetType == AssetCategory.Lands
                && role != SubLedgerAccountRole.Asset)
                throw new InvalidOperationException(InvalidBindingMessage);

            return;
        }

        if (fixedAssetType is not null
            || subLedgerType == SubLeadgerType.Tax
            || role is SubLedgerAccountRole.Asset
                or SubLedgerAccountRole.AccumulatedDepreciation
                or SubLedgerAccountRole.DepreciationExpense)
            throw new InvalidOperationException(InvalidBindingMessage);

        if (role == SubLedgerAccountRole.StockUnderTransfer)
        {
            if (subLedgerType != SubLeadgerType.Branch)
                throw new InvalidOperationException(InvalidBindingMessage);

            return;
        }

        if (role != SubLedgerAccountRole.Default)
            throw new InvalidOperationException(InvalidBindingMessage);
    }
}
