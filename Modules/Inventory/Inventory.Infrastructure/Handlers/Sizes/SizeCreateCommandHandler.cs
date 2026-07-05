using Inventory.Application.Commands.Sizes;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.Sizes;

namespace Inventory.Infrastructure.Handlers.Sizes;

public class SizeCreateCommandHandler(ISizeService service)
    : ICommandHandler<SizeCreateCommand, ApiResponse<Size>>
{
    public Task<ApiResponse<Size>> Handle(SizeCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
