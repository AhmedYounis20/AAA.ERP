using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Variants;
using ERP.Domain.Models.Entities.Inventory.Variants;

namespace ERP.Infrastracture.Handlers.Inventory.Variants;

public class VariantUpdateCommandHandler(IVariantService service) : ICommandHandler<VariantUpdateCommand, ApiResponse<Variant>>
{
    public async Task<ApiResponse<Variant>> Handle(VariantUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}