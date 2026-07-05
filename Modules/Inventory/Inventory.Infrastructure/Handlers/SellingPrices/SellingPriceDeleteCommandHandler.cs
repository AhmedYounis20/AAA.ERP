using Inventory.Application.Commands.SellingPrices;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.SellingPrices;

namespace Inventory.Infrastructure.Handlers.SellingPrices;

public class SellingPriceDeleteCommandHandler(ISellingPriceService service)
    : ICommandHandler<SellingPriceDeleteCommand, ApiResponse<SellingPrice>>
{
    public Task<ApiResponse<SellingPrice>> Handle(SellingPriceDeleteCommand request, CancellationToken cancellationToken)
        => service.Delete(request.Id);
}
