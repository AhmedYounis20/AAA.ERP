using Inventory.Domain.Commands.Colors;
using Inventory.Domain.Models.Entities.Colors;
using Shared;

namespace Inventory.Infrastructure.Handlers.Colors;

public class ColorCreateCommandHandler(IColorService service) : ICommandHandler<ColorCreateCommand, ApiResponse<Color>>
{
    public async Task<ApiResponse<Color>> Handle(ColorCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
} 
