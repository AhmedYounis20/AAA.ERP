using Accounting.Application.Commands.FixedAssets;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.FixedAssets;

public sealed class FixedAssetCreateCommandHandler(IFixedAssetWorkflowService workflow)
    : ICommandHandler<FixedAssetCreateCommand, ApiResponse<FixedAsset>>
{
    public Task<ApiResponse<FixedAsset>> Handle(FixedAssetCreateCommand request, CancellationToken cancellationToken)
        => workflow.Create(request);
}

public sealed class FixedAssetUpdateCommandHandler(IFixedAssetWorkflowService workflow)
    : ICommandHandler<FixedAssetUpdateCommand, ApiResponse<FixedAsset>>
{
    public Task<ApiResponse<FixedAsset>> Handle(FixedAssetUpdateCommand request, CancellationToken cancellationToken)
        => workflow.Update(request);
}

public sealed class FixedAssetDeleteCommandHandler(IFixedAssetWorkflowService workflow)
    : ICommandHandler<FixedAssetDeleteCommand, ApiResponse<FixedAsset>>
{
    public Task<ApiResponse<FixedAsset>> Handle(FixedAssetDeleteCommand request, CancellationToken cancellationToken)
        => workflow.Delete(request.Id);
}
