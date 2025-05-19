using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.PackingUnits;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;

namespace ERP.Infrastracture.Handlers.Inventory.PackingUnits;

public class PackingUnitUpdateCommandHandler(IPackingUnitService service) : ICommandHandler<PackingUnitUpdateCommand, ApiResponse<PackingUnit>>
{
    public async Task<ApiResponse<PackingUnit>> Handle(PackingUnitUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}