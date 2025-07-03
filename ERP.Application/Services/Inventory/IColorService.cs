using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Inventory.Colors;
using ERP.Domain.Models.Entities.Inventory.Colors;

namespace ERP.Application.Services.Inventory;

public interface IColorService : IBaseSettingService<Color, ColorCreateCommand, ColorUpdateCommand>
{
    Task<ApiResponse<string>> GetNextCodeAsync();

}
