using FluentValidation;
using Inventory.Domain.Models.DTOS;

namespace Inventory.Application.Validators.CommandValidators.Items;

public class ItemSellingPriceDiscountValidator : AbstractValidator<ItemSellingPriceDiscountDto>
{ 
    public ItemSellingPriceDiscountValidator() {

        _ = RuleFor(e => e.SellingPriceId).NotEmpty().WithMessage("SellingPriceIsRequired");
        _ = RuleFor(e => e.Discount).GreaterThanOrEqualTo(0).WithMessage("DiscountCannotBeNegative");
        _ = RuleFor(e => e.DiscountType).IsInEnum().WithMessage("DiscountTypeIsNotValid");
    }
}