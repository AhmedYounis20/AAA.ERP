using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.Sizes;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Application.Services;

/// <summary>
/// Application service contract for size.
/// </summary>
public interface ISizeService : IBaseSettingService<Size, SizeCreateCommand, SizeUpdateCommand>
{
    Task<ApiResponse<string>> GetNextCodeAsync();
}
