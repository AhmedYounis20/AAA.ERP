using Inventory.Application.Commands.PackingUnits;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.PackingUnits;

namespace Inventory.Infrastructure.Handlers.PackingUnits;

public class PackingUnitUpdateCommandHandler(IPackingUnitService service)
    : ICommandHandler<PackingUnitUpdateCommand, ApiResponse<PackingUnit>>
{
    public Task<ApiResponse<PackingUnit>> Handle(PackingUnitUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
