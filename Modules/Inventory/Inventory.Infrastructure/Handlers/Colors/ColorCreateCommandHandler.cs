using Inventory.Application.Commands.Colors;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.Colors;

namespace Inventory.Infrastructure.Handlers.Colors;

public class ColorCreateCommandHandler(IColorService service)
    : ICommandHandler<ColorCreateCommand, ApiResponse<Color>>
{
    public Task<ApiResponse<Color>> Handle(ColorCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
