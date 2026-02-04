using ERP.Domain.Models.Dtos.Inventory;
using FluentValidation;

namespace ERP.Application.Validators.Inventory.CommandValidators.Products;

public class ProductCostCenterValidator : AbstractValidator<ProductCostCenterDto>
{
    public ProductCostCenterValidator()
    {
        _ = RuleFor(e => e.CostCenterId).NotEmpty().WithMessage("CostCenterIsRequired");
        _ = RuleFor(e => e.Percent).GreaterThanOrEqualTo(0).WithMessage("PercentCannotBeNegative");
        _ = RuleFor(e => e.Percent).LessThanOrEqualTo(100).WithMessage("PercentCannotExceed100");
    }
}
