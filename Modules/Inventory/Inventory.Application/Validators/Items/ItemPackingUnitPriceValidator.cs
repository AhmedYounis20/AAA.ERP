using Inventory.Domain.Models.Dtos;
using Inventory.Domain.Models.Entities.Items;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Application.Validators.Items;

public class ItemPackingUnitPriceValidator : AbstractValidator<ItemPackingUnitSellingPriceDto>
{ 
    public ItemPackingUnitPriceValidator() {

        _ = RuleFor(e => e.SellingPriceId).NotEmpty().WithMessage(ValidationMessages.SellingPriceIsRequired);
        _ = RuleFor(e => e.Amount).GreaterThan(0).WithMessage(ValidationMessages.PriceAmountIsRequired);
    }
}