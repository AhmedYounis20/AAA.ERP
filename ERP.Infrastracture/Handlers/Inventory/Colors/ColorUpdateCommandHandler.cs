using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Colors;
using ERP.Domain.Models.Entities.Inventory.Colors;
using Shared;

namespace ERP.Infrastracture.Handlers.Inventory.Colors;

public class ColorUpdateCommandHandler(IColorService service) : ICommandHandler<ColorUpdateCommand, ApiResponse<Color>>
{
    public async Task<ApiResponse<Color>> Handle(ColorUpdateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
} 