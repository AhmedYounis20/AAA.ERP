using Inventory.Application.Services;
using Inventory.Application.Commands.Variants;
using ERP.Domain.Models.Entities.Inventory.Variants;

namespace Inventory.Infrastructure.Handlers.Variants;

public class VariantCreateCommandHandler(IVariantService service) : ICommandHandler<VariantCreateCommand, ApiResponse<Variant>>
{
    public async Task<ApiResponse<Variant>> Handle(VariantCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}