using Inventory.Domain.Models.Dtos;
using FluentValidation;

namespace Inventory.Application.Validators.Products;

public class ProductCostCenterValidator : AbstractValidator<ProductCostCenterDto>
{
    public ProductCostCenterValidator()
    {
        _ = RuleFor(e => e.CostCenterId).NotEmpty().WithMessage("CostCenterIsRequired");
        _ = RuleFor(e => e.Percent).GreaterThanOrEqualTo(0).WithMessage("PercentCannotBeNegative");
        _ = RuleFor(e => e.Percent).LessThanOrEqualTo(100).WithMessage("PercentCannotExceed100");
    }
}
