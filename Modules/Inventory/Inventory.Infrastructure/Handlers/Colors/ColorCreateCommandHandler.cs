using Inventory.Application.Commands.Colors;
using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.Colors;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Infrastructure.Handlers.Colors;

public class ColorCreateCommandHandler(IColorRepository repository)
    : ICommandHandler<ColorCreateCommand, ApiResponse<Color>>
{
    public async Task<ApiResponse<Color>> Handle(ColorCreateCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var entity = command.Adapt<Color>();
            var maxCode = await repository.GetMaxCodeAsync();
            var next = 1;
            if (int.TryParse(maxCode, out var max))
                next = max + 1;
            entity.Code = next.ToString();

            await repository.Add(entity);
            return new ApiResponse<Color>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Color>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }
}
