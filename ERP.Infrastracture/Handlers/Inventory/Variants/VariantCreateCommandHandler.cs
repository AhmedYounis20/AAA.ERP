using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Variants;
using ERP.Domain.Models.Entities.Inventory.Variants;

namespace ERP.Infrastracture.Handlers.Inventory.Variants;

public class VariantCreateCommandHandler(IVariantService service) : ICommandHandler<VariantCreateCommand, ApiResponse<Variant>>
{
    public async Task<ApiResponse<Variant>> Handle(VariantCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}