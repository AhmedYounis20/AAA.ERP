using Inventory.Application.Commands.Colors;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Colors;

namespace Inventory.Infrastructure.Handlers.Colors;

public class ColorDeleteCommandHandler(IColorService colorService)
    : ICommandHandler<ColorDeleteCommand, ApiResponse<Color>>
{
    public Task<ApiResponse<Color>> Handle(ColorDeleteCommand request, CancellationToken cancellationToken)
        => colorService.Delete(request.Id);
}
