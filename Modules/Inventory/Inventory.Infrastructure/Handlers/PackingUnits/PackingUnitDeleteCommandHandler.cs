using Inventory.Application.Commands.PackingUnits;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.PackingUnits;

namespace Inventory.Infrastructure.Handlers.PackingUnits;

public class PackingUnitDeleteCommandHandler(IPackingUnitService packingUnitService)
    : ICommandHandler<PackingUnitDeleteCommand, ApiResponse<PackingUnit>>
{
    public Task<ApiResponse<PackingUnit>> Handle(PackingUnitDeleteCommand request, CancellationToken cancellationToken)
        => packingUnitService.Delete(request.Id);
}
