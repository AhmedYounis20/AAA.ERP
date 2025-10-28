using Inventory.Domain.Commands.Sizes;
using Inventory.Domain.Models.Entities.Sizes;
using Shared;

namespace Inventory.Infrastructure.Handlers.Sizes;

public class SizeCreateCommandHandler(ISizeService service) : ICommandHandler<SizeCreateCommand, ApiResponse<Size>>
{
    public async Task<ApiResponse<Size>> Handle(SizeCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
} 
