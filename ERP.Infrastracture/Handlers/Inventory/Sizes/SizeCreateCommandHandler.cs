using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Sizes;
using ERP.Domain.Models.Entities.Inventory.Sizes;
using Shared;

namespace ERP.Infrastracture.Handlers.Inventory.Sizes;

public class SizeCreateCommandHandler(ISizeService service) : ICommandHandler<SizeCreateCommand, ApiResponse<Size>>
{
    public async Task<ApiResponse<Size>> Handle(SizeCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
} 