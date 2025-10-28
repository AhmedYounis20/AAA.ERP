using Inventory.Domain.Commands.Sizes;
using Inventory.Domain.Models.Entities.Sizes;
using Shared;

namespace Inventory.Infrastructure.Handlers.Sizes;

public class SizeUpdateCommandHandler(ISizeService service) : ICommandHandler<SizeUpdateCommand, ApiResponse<Size>>
{
    public async Task<ApiResponse<Size>> Handle(SizeUpdateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
} 
