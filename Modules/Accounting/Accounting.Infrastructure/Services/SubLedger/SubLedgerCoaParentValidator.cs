using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Infrastructure.Services.SubLedger;

public sealed class SubLedgerCoaParentValidator(IChartOfAccountRepository chartOfAccountRepository)
    : ISubLedgerCoaParentValidator
{
    public async Task<ChartOfAccount> ValidateAndGetAsync(
        Guid parentAccountId,
        CancellationToken cancellationToken = default)
    {
        if (parentAccountId == Guid.Empty)
            throw new InvalidOperationException(ErrorMessages.SubLedgerParentAccountNotFound);

        var account = await chartOfAccountRepository.Get(parentAccountId);
        if (account is null)
            throw new InvalidOperationException(ErrorMessages.SubLedgerParentAccountNotFound);

        if (account.IsCreatedFromSubLeadger
            || !account.IsActiveAccount
            || account.IsStopDealing)
            throw new InvalidOperationException(ErrorMessages.SubLedgerParentAccountInvalid);

        return account;
    }
}
