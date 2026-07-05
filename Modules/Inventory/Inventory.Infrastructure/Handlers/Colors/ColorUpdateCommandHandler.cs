using Inventory.Application.Commands.Colors;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.Colors;

namespace Inventory.Infrastructure.Handlers.Colors;

public class ColorUpdateCommandHandler(IColorService service)
    : ICommandHandler<ColorUpdateCommand, ApiResponse<Color>>
{
    public Task<ApiResponse<Color>> Handle(ColorUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
