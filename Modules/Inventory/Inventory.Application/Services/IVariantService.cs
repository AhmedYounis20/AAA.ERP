using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.Products;
using Inventory.Application.Commands.Variants;
using Inventory.Domain.Models.Entities.Variants;
using Inventory.Domain.Models.Dtos;
using Shared.DTOs;

namespace Inventory.Application.Services;

/// <summary>
/// Application service contract for variant.
/// </summary>
public interface IVariantService : IBaseService<Variant, VariantCreateCommand, VariantUpdateCommand>
{
    Task<ApiResponse<List<VariantDto>>> GetVariantDtos();
    Task<ApiResponse<VariantDto?>> GetVariantDtoById(Guid id);
    Task<ApiResponse<IEnumerable<VariantDto>>> GetVariantsByProductId(Guid productId);
    Task<ApiResponse<IEnumerable<VariantDto>>> SearchVariants(string searchTerm);
    Task<ApiResponse<PaginatedResult<VariantDto>>> ReadAllDtosPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> CreateVariantsBulkForProductUpdate(List<VariantCreateCommand> commands, bool isValidate = true);
    /// <summary>prepare variants for product operation.</summary>
    List<Variant> PrepareVariantsForProduct(ProductCreateCommand command);
}
