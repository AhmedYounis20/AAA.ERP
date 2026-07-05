using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Dtos;
using Inventory.Application.Services;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Variant read queries. Transitional: delegates to <see cref="IVariantService"/> until write methods are removed.
/// </summary>
public class VariantQueries(IVariantService variantService) : IVariantQueries
{
    public Task<ApiResponse<List<VariantDto>>> GetAll()
        => variantService.GetVariantDtos();

    public Task<ApiResponse<PaginatedResult<VariantDto>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default)
        => variantService.ReadAllDtosPaginated(filter, cancellationToken);

    public Task<ApiResponse<VariantDto?>> GetById(Guid id)
        => variantService.GetVariantDtoById(id);

    public Task<ApiResponse<IEnumerable<VariantDto>>> GetVariantsByProductId(Guid productId)
        => variantService.GetVariantsByProductId(productId);

    public Task<ApiResponse<IEnumerable<VariantDto>>> SearchVariants(string searchTerm)
        => variantService.SearchVariants(searchTerm);
}
