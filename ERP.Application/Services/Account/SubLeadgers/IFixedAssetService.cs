using ERP.Domain.Commands.Account.SubLeadgers.FixedAssets;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using Shared.Responses;

namespace ERP.Application.Services.Account.SubLeadgers;

public interface
    IFixedAssetService : IBaseSubLeadgerService<FixedAsset, FixedAssetCreateCommand, FixedAssetUpdateCommand>
{
    public Task<ApiResponse<FixedAssetCreateCommand>> GetNextFixedAsset(Guid? parentId, FixedAssetType fixedAssetType);

}