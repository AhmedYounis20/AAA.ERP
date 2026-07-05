using Accounting.Application.Commands.FixedAssets;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.FixedAssets;

public class FixedAssetUpdateCommandHandler(IFixedAssetService service)
    : ICommandHandler<FixedAssetUpdateCommand, ApiResponse<FixedAsset>>
{
    public Task<ApiResponse<FixedAsset>> Handle(FixedAssetUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
