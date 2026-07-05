using Inventory.Domain.Models.Dtos;
using Inventory.Domain.Models.Entities.Items;
using FluentValidation;

namespace Inventory.Application.Validators.Items;

public class ItemSellingPriceDiscountValidator : AbstractValidator<ItemSellingPriceDiscountDto>
{ 
    public ItemSellingPriceDiscountValidator() {

        _ = RuleFor(e => e.SellingPriceId).NotEmpty().WithMessage("SellingPriceIsRequired");
        _ = RuleFor(e => e.Discount).GreaterThanOrEqualTo(0).WithMessage("DiscountCannotBeNegative");
        _ = RuleFor(e => e.DiscountType).IsInEnum().WithMessage("DiscountTypeIsNotValid");
    }
}