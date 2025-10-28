using Account.Application.Services.SubLeadgers;
using Account.Domain.Commands.SubLeadgers.FixedAssets;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Infrastructure.Handlers.FixedAssets;

public class FixedAssetUpdateCommandHandler(IFixedAssetService service) : ICommandHandler<FixedAssetUpdateCommand, ApiResponse<FixedAsset>>
{
    public async Task<ApiResponse<FixedAsset>> Handle(FixedAssetUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
