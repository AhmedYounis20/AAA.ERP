using Accounting.Application.Commands.FixedAssets;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.FixedAssets;

public class FixedAssetDeleteCommandHandler(IFixedAssetService service)
    : ICommandHandler<FixedAssetDeleteCommand, ApiResponse<FixedAsset>>
{
    public Task<ApiResponse<FixedAsset>> Handle(FixedAssetDeleteCommand request, CancellationToken cancellationToken)
        => service.Delete(request.Id);
}
