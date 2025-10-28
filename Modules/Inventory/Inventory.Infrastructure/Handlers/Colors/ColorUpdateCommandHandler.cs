using Inventory.Domain.Commands.Colors;
using Inventory.Domain.Models.Entities.Colors;
using Shared;

namespace Inventory.Infrastructure.Handlers.Colors;

public class ColorUpdateCommandHandler(IColorService service) : ICommandHandler<ColorUpdateCommand, ApiResponse<Color>>
{
    public async Task<ApiResponse<Color>> Handle(ColorUpdateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
} 
