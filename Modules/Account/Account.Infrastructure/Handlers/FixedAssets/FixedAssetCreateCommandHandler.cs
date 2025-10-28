using Account.Application.Services.SubLeadgers;
using Account.Domain.Commands.SubLeadgers.FixedAssets;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Infrastructure.Handlers.FixedAssets;

public class FixedAssetCreateCommandHandler(IFixedAssetService service) : ICommandHandler<FixedAssetCreateCommand, ApiResponse<FixedAsset>>
{
    public async Task<ApiResponse<FixedAsset>> Handle(FixedAssetCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
