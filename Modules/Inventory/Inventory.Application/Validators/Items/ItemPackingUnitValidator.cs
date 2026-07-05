using Inventory.Application.Dtos;
using Inventory.Domain.Models.Entities.Items;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Application.Validators.Items;

public class ItemPackingUnitValidator : AbstractValidator<ItemPackingUnitDto>
{
    public ItemPackingUnitValidator()
    {

        _ = RuleFor(e => e.PackingUnitId).NotEmpty().WithMessage(ValidationMessages.PackingUnitIsRequired2);
        _ = RuleFor(e => e.AverageCostPrice).GreaterThan(0).WithMessage(ValidationMessages.AverageCostIsRequired);
        _ = RuleFor(e => e.LastCostPrice).GreaterThan(0).WithMessage(ValidationMessages.LastCostPriceIsRequired);
        _ = RuleFor(e => e.PartsCount).GreaterThan(0).WithMessage(ValidationMessages.PartsCountIsRequired);
        _ = RuleFor(e => e.SellingPrices).NotEmpty().WithMessage(ValidationMessages.PackingUnitSellingPriceIsRequired);
        _ = RuleForEach(e => e.SellingPrices).SetValidator(new ItemPackingUnitPriceValidator()).WithMessage(ValidationMessages.PartsCountIsRequired);
    }
}