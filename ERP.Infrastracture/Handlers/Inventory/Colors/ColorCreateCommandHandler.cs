using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Colors;
using ERP.Domain.Models.Entities.Inventory.Colors;
using Shared;

namespace ERP.Infrastracture.Handlers.Inventory.Colors;

public class ColorCreateCommandHandler(IColorService service) : ICommandHandler<ColorCreateCommand, ApiResponse<Color>>
{
    public async Task<ApiResponse<Color>> Handle(ColorCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
} 