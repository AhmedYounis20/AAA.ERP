using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Commands.Sizes;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Sizes;
using ERP.Infrastructure.Services.Queries;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Size read queries. Transitional: delegates to <see cref="ISizeService"/> until write methods are removed.
/// </summary>
public class SizeQueries(ISizeService sizeService)
    : BaseSettingQueries<Size, SizeCreateCommand, SizeUpdateCommand>(sizeService),
      ISizeQueries
{
    public Task<ApiResponse<string>> GetNextCode()
        => sizeService.GetNextCodeAsync();
}
