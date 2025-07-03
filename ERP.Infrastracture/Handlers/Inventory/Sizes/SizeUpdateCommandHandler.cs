using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Sizes;
using ERP.Domain.Models.Entities.Inventory.Sizes;
using Shared;

namespace ERP.Infrastracture.Handlers.Inventory.Sizes;

public class SizeUpdateCommandHandler(ISizeService service) : ICommandHandler<SizeUpdateCommand, ApiResponse<Size>>
{
    public async Task<ApiResponse<Size>> Handle(SizeUpdateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
} 