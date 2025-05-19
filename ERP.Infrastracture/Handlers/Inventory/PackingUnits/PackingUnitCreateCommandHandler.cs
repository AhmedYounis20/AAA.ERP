using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.PackingUnits;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;

namespace ERP.Infrastracture.Handlers.Inventory.PackingUnits;

public class PackingUnitCreateCommandHandler(IPackingUnitService service) : ICommandHandler<PackingUnitCreateCommand, ApiResponse<PackingUnit>>
{
    public async Task<ApiResponse<PackingUnit>> Handle(PackingUnitCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}