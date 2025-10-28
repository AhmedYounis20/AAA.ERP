using Inventory.Domain.Commands.PackingUnits;
using Inventory.Domain.Models.Entities.PackingUnits;

namespace Inventory.Infrastructure.Handlers.PackingUnits;

public class PackingUnitUpdateCommandHandler(IPackingUnitService service) : ICommandHandler<PackingUnitUpdateCommand, ApiResponse<PackingUnit>>
{
    public async Task<ApiResponse<PackingUnit>> Handle(PackingUnitUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}