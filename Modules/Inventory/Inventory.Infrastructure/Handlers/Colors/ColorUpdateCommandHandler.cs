using Inventory.Application.Commands.Colors;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Colors;

namespace Inventory.Infrastructure.Handlers.Colors;

public class ColorUpdateCommandHandler(IColorService service)
    : ICommandHandler<ColorUpdateCommand, ApiResponse<Color>>
{
    public Task<ApiResponse<Color>> Handle(ColorUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
