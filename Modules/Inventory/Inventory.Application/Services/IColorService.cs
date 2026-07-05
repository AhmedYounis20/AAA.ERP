using ERP.Application.Services.BaseServices;
using Inventory.Application.Commands.Colors;
using ERP.Domain.Models.Entities.Inventory.Colors;

namespace Inventory.Application.Services;

public interface IColorService : IBaseSettingService<Color, ColorCreateCommand, ColorUpdateCommand>
{
    Task<ApiResponse<string>> GetNextCodeAsync();
}
