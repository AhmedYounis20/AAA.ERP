using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.Sizes;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Application.Services;

public interface ISizeService : IBaseSettingService<Size, SizeCreateCommand, SizeUpdateCommand>
{
    Task<ApiResponse<string>> GetNextCodeAsync();
}
