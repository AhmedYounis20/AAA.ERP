using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Commands.SubLeadgers.FixedAssets;
using Domain.Account.Models.Entities.SubLeadgers;
using Shared.Responses;

namespace Domain.Account.Services.Interfaces.SubLeadgers;

public interface
    IFixedAssetService : IBaseSubLeadgerService<FixedAsset, FixedAssetCreateCommand, FixedAssetUpdateCommand>
{
    public Task<ApiResponse<FixedAssetCreateCommand>> GetNextFixedAsset(Guid? parentId,FixedAssetType fixedAssetType);

}