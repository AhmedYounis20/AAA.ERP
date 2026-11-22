using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Infrastructure.Services.SubLedger;

public sealed class SubLedgerCoaConfigurationValidator(ISubLedgerBaseAccountRepository baseAccountRepository)
    : ISubLedgerCoaConfigurationValidator
{
    public async Task ValidateAsync(CancellationToken cancellationToken = default)
    {
        var bindings = await baseAccountRepository.ListAsync(cancellationToken);
        var missing = new List<string>();

        foreach (var requirement in GetRequirements())
        {
            var matches = bindings.Count(binding => MatchesRequirement(binding, requirement));

            if (matches == 0)
            {
                missing.Add(FormatRequirement(requirement));
                continue;
            }

            if (matches > 1)
                throw new InvalidOperationException(
                    $"{ErrorMessages.SubLedgerBaseAccountAmbiguous}: {FormatRequirement(requirement)}");
        }

        if (missing.Count > 0)
        {
            throw new InvalidOperationException(
                $"{ErrorMessages.SubLedgerBaseAccountNotConfigured}: {string.Join("; ", missing)}");
        }

        foreach (var binding in bindings)
            SubLedgerBaseAccount.EnsureCombination(binding.SubLedgerType, binding.Role, binding.FixedAssetType);
    }

    private static IEnumerable<SubLedgerCoaRequirement> GetRequirements()
    {
        foreach (SubLeadgerType type in Enum.GetValues<SubLeadgerType>())
        {
            if (type == SubLeadgerType.Tax || type == SubLeadgerType.FixedAsset)
                continue;

            yield return new SubLedgerCoaRequirement(type, SubLedgerAccountRole.Default, null);
        }

        yield return new SubLedgerCoaRequirement(
            SubLeadgerType.Branch,
            SubLedgerAccountRole.StockUnderTransfer,
            null);

        foreach (FixedAssetType assetType in Enum.GetValues<FixedAssetType>())
        {
            yield return new SubLedgerCoaRequirement(
                SubLeadgerType.FixedAsset,
                SubLedgerAccountRole.Asset,
                assetType);

            if (assetType == FixedAssetType.Lands)
                continue;

            yield return new SubLedgerCoaRequirement(
                SubLeadgerType.FixedAsset,
                SubLedgerAccountRole.AccumulatedDepreciation,
                assetType);

            yield return new SubLedgerCoaRequirement(
                SubLeadgerType.FixedAsset,
                SubLedgerAccountRole.DepreciationExpense,
                assetType);
        }
    }

    private static bool MatchesRequirement(SubLedgerBaseAccount binding, SubLedgerCoaRequirement requirement)
        => binding.SubLedgerType == requirement.SubLedgerType
           && binding.Role == requirement.Role
           && binding.FixedAssetType == requirement.FixedAssetType;

    private static string FormatRequirement(SubLedgerCoaRequirement requirement)
        => requirement.FixedAssetType is null
            ? $"{requirement.SubLedgerType}/{requirement.Role}"
            : $"{requirement.SubLedgerType}/{requirement.FixedAssetType}/{requirement.Role}";

    private sealed record SubLedgerCoaRequirement(
        SubLeadgerType SubLedgerType,
        SubLedgerAccountRole Role,
        FixedAssetType? FixedAssetType);
}
