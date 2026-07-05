using ERP.Application.Services.BaseServices;
using Inventory.Application.Commands.Sizes;
using ERP.Domain.Models.Entities.Inventory.Sizes;

namespace Inventory.Application.Services;

public interface ISizeService : IBaseSettingService<Size, SizeCreateCommand, SizeUpdateCommand>
{
    Task<ApiResponse<string>> GetNextCodeAsync();
}
