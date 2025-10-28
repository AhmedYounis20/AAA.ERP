using Inventory.Domain.Models.Entities.Sizes;
using Inventory.Domain.Commands.Sizes;
using Shared.Application.BaseServices;
using Shared.Domain.Responses;

namespace Inventory.Application.Services;

public interface ISizeService : IBaseSettingService<Size, SizeCreateCommand, SizeUpdateCommand>
{
    Task<ApiResponse<string>> GetNextCodeAsync();
}
