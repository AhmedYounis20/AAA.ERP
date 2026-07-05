using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.Colors;
using Inventory.Domain.Models.Entities.Colors;

namespace Inventory.Application.Services;

public interface IColorService : IBaseSettingService<Color, ColorCreateCommand, ColorUpdateCommand>
{
    Task<ApiResponse<string>> GetNextCodeAsync();
}
