using Inventory.Domain.Models.Entities.Colors;
using Inventory.Domain.Commands.Colors;
using Shared.Application.BaseServices;
using Shared.Domain.Responses;

namespace Inventory.Application.Services;

public interface IColorService : IBaseSettingService<Color, ColorCreateCommand, ColorUpdateCommand>
{
    Task<ApiResponse<string>> GetNextCodeAsync();

}
