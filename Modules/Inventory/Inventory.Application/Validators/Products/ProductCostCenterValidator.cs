using Inventory.Domain.Models.Dtos;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Application.Validators.Products;

public class ProductCostCenterValidator : AbstractValidator<ProductCostCenterDto>
{
    public ProductCostCenterValidator()
    {
        _ = RuleFor(e => e.CostCenterId).NotEmpty().WithMessage(ValidationMessages.CostCenterIsRequired);
        _ = RuleFor(e => e.Percent).GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.PercentCannotBeNegative);
        _ = RuleFor(e => e.Percent).LessThanOrEqualTo(100).WithMessage(ValidationMessages.PercentCannotExceed100);
    }
}
