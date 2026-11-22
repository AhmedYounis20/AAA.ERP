using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Infrastructure.Services.SubLedger;

public sealed class SubLedgerCoaParentResolver(
    ISubLedgerBaseAccountRepository baseAccountRepository) : ISubLedgerCoaParentResolver
{
    public async Task<ChartOfAccount> ResolveAsync(
        SubLeadgerType subLedgerType,
        FixedAssetType? fixedAssetType = null,
        SubLedgerAccountRole role = SubLedgerAccountRole.Default,
        CancellationToken cancellationToken = default)
    {
        var matches = await baseAccountRepository.FindAsync(
            subLedgerType,
            role,
            fixedAssetType,
            cancellationToken);

        if (matches.Count == 0)
            throw new InvalidOperationException(ErrorMessages.SubLedgerBaseAccountNotConfigured);

        if (matches.Count > 1)
            throw new InvalidOperationException(ErrorMessages.SubLedgerBaseAccountAmbiguous);

        var account = matches[0].ChartOfAccount;
        if (account is null)
            throw new InvalidOperationException(ErrorMessages.SubLedgerBaseAccountNotConfigured);

        return account;
    }
}
