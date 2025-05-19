using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Commands.SubLeadgers.FixedAssets;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Services.Account.SubLeadgers;
using Shared;
using Shared.Responses;

namespace ERP.Infrastracture.Handlers.Banks;

public class FixedAssetUpdateCommandHandler(IFixedAssetService service): ICommandHandler<FixedAssetUpdateCommand,ApiResponse<FixedAsset>>
{
    public async Task<ApiResponse<FixedAsset>> Handle(FixedAssetUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}