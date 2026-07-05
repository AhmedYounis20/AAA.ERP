using Accounting.Application.Commands.FixedAssets;
using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using Shared.Responses;

namespace Accounting.Application.Services;

public interface IFixedAssetService : IBaseSubLeadgerService<FixedAsset, FixedAssetCreateCommand, FixedAssetUpdateCommand>
{
    Task<ApiResponse<FixedAssetCreateCommand>> GetNextFixedAsset(Guid? parentId, FixedAssetType fixedAssetType);
}
