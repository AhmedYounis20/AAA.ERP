using Accounting.Application.Commands.FixedAssets;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Utility;
using ERP.BuildingBlocks.Domain.Entities;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Fixed-asset default-data helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public class FixedAssetService(IChartOfAccountRepository chartOfAccountRepository) : IFixedAssetService
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
