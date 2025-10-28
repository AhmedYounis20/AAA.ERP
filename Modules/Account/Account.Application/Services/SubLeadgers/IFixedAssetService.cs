using Account.Domain.Commands.SubLeadgers.FixedAssets;
using Account.Domain.Models.Entities.SubLeadgers;
using Shared.Domain.Responses;

namespace Account.Application.Services.SubLeadgers;

public interface
    IFixedAssetService : IBaseSubLeadgerService<FixedAsset, FixedAssetCreateCommand, FixedAssetUpdateCommand>
{
    public Task<ApiResponse<FixedAssetCreateCommand>> GetNextFixedAsset(Guid? parentId, FixedAssetType fixedAssetType);

}