using Accounting.Application.Commands.FixedAssets;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Utility;
using ERP.BuildingBlocks.Domain.Entities;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Fixed-asset read queries.
/// </summary>
public class FixedAssetQueries(IFixedAssetRepository repository, IChartOfAccountRepository chartOfAccountRepository)
    : BaseTreeSettingRepositoryQueries<FixedAsset>(repository),
      IFixedAssetQueries
{
    public async Task<ApiResponse<FixedAssetCreateCommand>> GetNextFixedAsset(
        Guid? parentId,
        FixedAssetType fixedAssetType)
    {
        var chartOfAccountId = Guid.Parse(SD.FixedAssetsIds[fixedAssetType]);
        var assetCodes = SD.FixedAssetsDepreciationDetails[fixedAssetType];
        var code = await chartOfAccountRepository.GenerateNewCodeForChild(chartOfAccountId);
        var accumelatedCode = fixedAssetType == FixedAssetType.Lands
            ? ""
            : await chartOfAccountRepository.GenerateNewCodeForChild(Guid.Parse(assetCodes[0]));
        var expenseCode = fixedAssetType == FixedAssetType.Lands
            ? ""
            : await chartOfAccountRepository.GenerateNewCodeForChild(Guid.Parse(assetCodes[1]));
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
