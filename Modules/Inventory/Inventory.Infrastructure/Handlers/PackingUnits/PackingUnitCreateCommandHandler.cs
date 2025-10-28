using Inventory.Domain.Commands.PackingUnits;
using Inventory.Domain.Models.Entities.PackingUnits;

namespace Inventory.Infrastructure.Handlers.PackingUnits;

public class PackingUnitCreateCommandHandler(IPackingUnitService service) : ICommandHandler<PackingUnitCreateCommand, ApiResponse<PackingUnit>>
{
    public async Task<ApiResponse<PackingUnit>> Handle(PackingUnitCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
