using Inventory.Application.Commands.Sizes;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Infrastructure.Handlers.Sizes;

public class SizeDeleteCommandHandler(ISizeService sizeService)
    : ICommandHandler<SizeDeleteCommand, ApiResponse<Size>>
{
    public Task<ApiResponse<Size>> Handle(SizeDeleteCommand request, CancellationToken cancellationToken)
        => sizeService.Delete(request.Id);
}
