using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.FixedAssets;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Infrastracture.Handlers.Account.FixedAssets;

public class FixedAssetUpdateCommandHandler(IFixedAssetService service) : ICommandHandler<FixedAssetUpdateCommand, ApiResponse<FixedAsset>>
{
    public async Task<ApiResponse<FixedAsset>> Handle(FixedAssetUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}