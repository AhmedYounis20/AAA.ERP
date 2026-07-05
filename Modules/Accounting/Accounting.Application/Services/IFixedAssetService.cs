using Accounting.Application.Commands.FixedAssets;
using Accounting.Application.Services.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Shared.Responses;

namespace Accounting.Application.Services;

public interface IFixedAssetService : IBaseSubLeadgerService<FixedAsset, FixedAssetCreateCommand, FixedAssetUpdateCommand>
{
    Task<ApiResponse<FixedAssetCreateCommand>> GetNextFixedAsset(Guid? parentId, FixedAssetType fixedAssetType);
}
