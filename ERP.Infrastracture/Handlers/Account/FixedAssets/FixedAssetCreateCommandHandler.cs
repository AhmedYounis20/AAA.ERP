using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.FixedAssets;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Infrastracture.Handlers.Account.FixedAssets;

public class FixedAssetCreateCommandHandler(IFixedAssetService service) : ICommandHandler<FixedAssetCreateCommand, ApiResponse<FixedAsset>>
{
    public async Task<ApiResponse<FixedAsset>> Handle(FixedAssetCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}