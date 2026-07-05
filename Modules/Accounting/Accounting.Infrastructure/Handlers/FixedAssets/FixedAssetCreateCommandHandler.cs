using Accounting.Application.Commands.FixedAssets;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.FixedAssets;

public class FixedAssetCreateCommandHandler(IFixedAssetService service)
    : ICommandHandler<FixedAssetCreateCommand, ApiResponse<FixedAsset>>
{
    public Task<ApiResponse<FixedAsset>> Handle(FixedAssetCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
