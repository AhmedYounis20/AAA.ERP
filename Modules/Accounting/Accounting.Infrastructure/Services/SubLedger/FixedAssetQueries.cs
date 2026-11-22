using Accounting.Application.Commands.FixedAssets;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Domain.Entities;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Fixed-asset read queries.
/// </summary>
public class FixedAssetQueries(
    IFixedAssetRepository repository,
    IChartOfAccountRepository chartOfAccountRepository,
    ISubLedgerCoaParentResolver subLedgerCoaParentResolver)
    : BaseTreeSettingRepositoryQueries<FixedAsset>(repository),
      IFixedAssetQueries
{
    public async Task<ApiResponse<FixedAssetCreateCommand>> GetNextFixedAsset(
        Guid? parentId,
        FixedAssetType fixedAssetType)
    {
        var assetParent = await subLedgerCoaParentResolver.ResolveAsync(
            SubLeadgerType.FixedAsset,
            fixedAssetType,
            SubLedgerAccountRole.Asset);

        var code = await chartOfAccountRepository.GenerateNewCodeForChild(assetParent.Id);
        var accumelatedCode = fixedAssetType == FixedAssetType.Lands
            ? string.Empty
            : await chartOfAccountRepository.GenerateNewCodeForChild(
                (await subLedgerCoaParentResolver.ResolveAsync(
                    SubLeadgerType.FixedAsset,
                    fixedAssetType,
                    SubLedgerAccountRole.AccumulatedDepreciation)).Id);
        var expenseCode = fixedAssetType == FixedAssetType.Lands
            ? string.Empty
            : await chartOfAccountRepository.GenerateNewCodeForChild(
                (await subLedgerCoaParentResolver.ResolveAsync(
                    SubLeadgerType.FixedAsset,
                    fixedAssetType,
                    SubLedgerAccountRole.DepreciationExpense)).Id);

        var inputModel = Activator.CreateInstance<FixedAssetCreateCommand>();
        inputModel.Code = code;
        inputModel.ExpensesCode = expenseCode;
        inputModel.AccumelatedCode = accumelatedCode;
        inputModel.NodeType = NodeType.Domain;
        inputModel.ParentId = parentId;
        inputModel.FixedAssetType = fixedAssetType;

        return new ApiResponse<FixedAssetCreateCommand>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = inputModel
        };
    }
}
