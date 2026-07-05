using Inventory.Application.Commands.PackingUnits;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;

namespace Inventory.Infrastructure.Handlers.PackingUnits;

public class PackingUnitCreateCommandHandler(IPackingUnitService service)
    : ICommandHandler<PackingUnitCreateCommand, ApiResponse<PackingUnit>>
{
    public Task<ApiResponse<PackingUnit>> Handle(PackingUnitCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
