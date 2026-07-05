using ERP.Application.Services.BaseServices;
using Inventory.Application.Commands.Products;
using Inventory.Application.Commands.Variants;
using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Domain.Models.Dtos.Inventory;
using Shared.DTOs;

namespace Inventory.Application.Services;

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
    List<Variant> PrepareVariantsForProduct(ProductCreateCommand command);
}
