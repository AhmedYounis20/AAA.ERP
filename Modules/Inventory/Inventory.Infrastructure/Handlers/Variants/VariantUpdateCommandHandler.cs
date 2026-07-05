using Inventory.Application.Services;
using Inventory.Application.Commands.Variants;
using Inventory.Domain.Models.Entities.Variants;

namespace Inventory.Infrastructure.Handlers.Variants;

public class VariantUpdateCommandHandler(IVariantService service) : ICommandHandler<VariantUpdateCommand, ApiResponse<Variant>>
{
    public async Task<ApiResponse<Variant>> Handle(VariantUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}