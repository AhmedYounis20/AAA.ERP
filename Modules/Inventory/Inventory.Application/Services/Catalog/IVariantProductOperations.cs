using Inventory.Application.Commands.Products;
using Inventory.Application.Commands.Variants;
using Inventory.Domain.Models.Entities.Variants;
using ERP.BuildingBlocks.Application.Responses;

namespace Inventory.Application.Services;

/// <summary>
/// Variant operations invoked from product write orchestration.
/// </summary>
public interface IVariantProductOperations
{
    List<Variant> PrepareVariantsForProduct(ProductCreateCommand command);

    Task<ApiResponse<bool>> CreateVariantsBulkForProductUpdate(
        List<VariantCreateCommand> commands,
        bool isValidate = true);
}
