using Inventory.Application.Commands.PackingUnits;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;

namespace Inventory.Infrastructure.Handlers.PackingUnits;

public class PackingUnitUpdateCommandHandler(IPackingUnitService service)
    : ICommandHandler<PackingUnitUpdateCommand, ApiResponse<PackingUnit>>
{
    public Task<ApiResponse<PackingUnit>> Handle(PackingUnitUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
