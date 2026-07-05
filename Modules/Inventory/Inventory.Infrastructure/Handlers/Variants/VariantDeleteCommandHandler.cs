using Inventory.Application.Commands.Variants;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Variants;

namespace Inventory.Infrastructure.Handlers.Variants;

public class VariantDeleteCommandHandler(IVariantService variantService)
    : ICommandHandler<VariantDeleteCommand, ApiResponse<Variant>>
{
    public Task<ApiResponse<Variant>> Handle(VariantDeleteCommand request, CancellationToken cancellationToken)
        => variantService.Delete(request.Id);
}
