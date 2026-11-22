using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Infrastructure.Services.SubLedger;

public sealed class SubLedgerCoaFactory(IChartOfAccountRepository chartOfAccountRepository) : ISubLedgerCoaFactory
{
    public async Task<ChartOfAccount> CreateChildAccountAsync(
        string name,
        string? nameSecondLanguage,
        ChartOfAccount parent,
        SubLeadgerType subLedgerType,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (parent is null)
            throw new InvalidOperationException(ErrorMessages.SubLedgerParentAccountNotFound);

        var newCode = await chartOfAccountRepository.GenerateNewCodeForChild(parent.Id);

        return new ChartOfAccount
        {
            Name = name,
            NameSecondLanguage = nameSecondLanguage,
            ParentId = parent.Id,
            Code = newCode,
            AccountNature = parent.AccountNature,
            IsDepreciable = parent.IsDepreciable,
            IsActiveAccount = parent.IsActiveAccount,
            IsStopDealing = parent.IsStopDealing,
            IsPostedAccount = parent.IsPostedAccount,
            AccountGuidId = parent.AccountGuidId,
            IsCreatedFromSubLeadger = true,
            SubLeadgerType = subLedgerType
        };
    }

    public async Task<ChartOfAccount> CreatePrefixedChildAccountAsync(
        ChartOfAccount namePrefixSource,
        string entityName,
        string? entityNameSecondLanguage,
        ChartOfAccount parent,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(entityName);
        if (namePrefixSource is null || parent is null)
            throw new InvalidOperationException(ErrorMessages.SubLedgerParentAccountNotFound);

        var newCode = await chartOfAccountRepository.GenerateNewCodeForChild(parent.Id);

        return new ChartOfAccount
        {
            Name = $"{namePrefixSource.Name} {entityName}",
            NameSecondLanguage = $"{namePrefixSource.NameSecondLanguage} {entityNameSecondLanguage}",
            ParentId = parent.Id,
            Code = newCode,
            AccountNature = parent.AccountNature,
            IsDepreciable = parent.IsDepreciable,
            IsActiveAccount = parent.IsActiveAccount,
            IsStopDealing = parent.IsStopDealing,
            IsPostedAccount = parent.IsPostedAccount,
            AccountGuidId = parent.AccountGuidId,
            IsCreatedFromSubLeadger = true,
            SubLeadgerType = parent.SubLeadgerType
        };
    }
}
