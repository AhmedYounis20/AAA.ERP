using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Inventory.Variants;
using ERP.Domain.Commands.Inventory.Products;
using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Domain.Models.Dtos.Inventory;

namespace ERP.Application.Services.Inventory;

public interface IVariantService : IBaseService<Variant, VariantCreateCommand, VariantUpdateCommand>
{
    Task<ApiResponse<List<VariantDto>>> GetVariantDtos();
    Task<ApiResponse<VariantDto?>> GetVariantDtoById(Guid id);
    Task<ApiResponse<IEnumerable<VariantDto>>> GetVariantsByProductId(Guid productId);
    Task<ApiResponse<IEnumerable<VariantDto>>> SearchVariants(string searchTerm);
    List<Variant> PrepareVariantsForProduct(ProductCreateCommand command);

    Task<ApiResponse<bool>> CreateVariantsBulkForProductUpdate( List<VariantCreateCommand> commands, bool isValidate = true);
}