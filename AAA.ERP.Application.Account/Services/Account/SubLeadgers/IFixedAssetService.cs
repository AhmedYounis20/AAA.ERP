using Domain.Account.Commands.SubLeadgers.FixedAssets;
using Domain.Account.Models.Entities.SubLeadgers;
using Shared.Responses;

namespace ERP.Application.Services.Account.SubLeadgers;

public interface
    IFixedAssetService : IBaseSubLeadgerService<FixedAsset, FixedAssetCreateCommand, FixedAssetUpdateCommand>
{
    public Task<ApiResponse<FixedAssetCreateCommand>> GetNextFixedAsset(Guid? parentId, FixedAssetType fixedAssetType);

}