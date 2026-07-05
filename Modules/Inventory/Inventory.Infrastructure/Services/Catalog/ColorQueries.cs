using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Commands.Colors;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Colors;
using ERP.Infrastructure.Services.Queries;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Color read queries. Transitional: delegates to <see cref="IColorService"/> until write methods are removed.
/// </summary>
public class ColorQueries(IColorService colorService)
    : BaseSettingQueries<Color, ColorCreateCommand, ColorUpdateCommand>(colorService),
      IColorQueries
{
    public Task<ApiResponse<string>> GetNextCode()
        => colorService.GetNextCodeAsync();
}
