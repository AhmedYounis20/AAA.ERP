using Inventory.Application.Commands.Sizes;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Infrastructure.Handlers.Sizes;

public class SizeUpdateCommandHandler(ISizeService service)
    : ICommandHandler<SizeUpdateCommand, ApiResponse<Size>>
{
    public Task<ApiResponse<Size>> Handle(SizeUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
