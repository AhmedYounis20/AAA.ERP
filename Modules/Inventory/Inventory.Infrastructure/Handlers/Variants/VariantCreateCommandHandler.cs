using Inventory.Application.Commands.Variants;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Variants;

namespace Inventory.Infrastructure.Handlers.Variants;

public sealed class VariantCreateCommandHandler(IVariantWorkflowService workflow)
    : ICommandHandler<VariantCreateCommand, ApiResponse<Variant>>
{
    public Task<ApiResponse<Variant>> Handle(VariantCreateCommand request, CancellationToken cancellationToken)
        => workflow.Create(request);
}

public sealed class VariantUpdateCommandHandler(IVariantWorkflowService workflow)
    : ICommandHandler<VariantUpdateCommand, ApiResponse<Variant>>
{
    public Task<ApiResponse<Variant>> Handle(VariantUpdateCommand request, CancellationToken cancellationToken)
        => workflow.Update(request);
}

public sealed class VariantDeleteCommandHandler(IVariantWorkflowService workflow)
    : ICommandHandler<VariantDeleteCommand, ApiResponse<Variant>>
{
    public Task<ApiResponse<Variant>> Handle(VariantDeleteCommand request, CancellationToken cancellationToken)
        => workflow.Delete(request.Id);
}