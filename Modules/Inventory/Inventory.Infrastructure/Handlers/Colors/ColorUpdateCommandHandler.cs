using Inventory.Application.Commands.Colors;
using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.Colors;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Infrastructure.Handlers.Colors;

public class ColorUpdateCommandHandler(IColorRepository repository)
    : ICommandHandler<ColorUpdateCommand, ApiResponse<Color>>
{
    public async Task<ApiResponse<Color>> Handle(ColorUpdateCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await repository.Get(command.Id);
            if (entity is null)
            {
                return new ApiResponse<Color>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Errors = [new MessageTemplate { MessageKey = ErrorMessages.RecordNotFound }]
                };
            }

            var updated = command.Adapt<Color>();
            updated.ModifiedAt = DateTime.Now;
            await repository.Update(updated);

            return new ApiResponse<Color>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = updated
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
