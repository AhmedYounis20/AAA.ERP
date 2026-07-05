using Inventory.Application.Commands.Products;
using ERP.Domain.Models.Entities.Inventory.Variants;

namespace Inventory.Application.Services;

/// <summary>
/// Cross-slice hook used by ProductService until Variants migrate (Wave 3D Week 18).
/// </summary>
public interface IVariantProductPreparation
{
    List<Variant> PrepareVariantsForProduct(ProductCreateCommand command);
}
