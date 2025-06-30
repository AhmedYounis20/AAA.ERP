using ERP.Domain.Models.Entities.Inventory.Items;
using FluentValidation;

namespace ERP.Application.Validators.Inventory.CommandValidators.Items;

public class ItemSellingPriceDiscountValidator : AbstractValidator<ItemSellingPriceDiscountDto>
{ 
    public ItemSellingPriceDiscountValidator() {

        _ = RuleFor(e => e.SellingPriceId).NotEmpty().WithMessage("SellingPriceIsRequired");
        _ = RuleFor(e => e.Discount).GreaterThan(0).WithMessage("DiscountCannotBeNegative");
        _ = RuleFor(e => e.DiscountType).IsInEnum().WithMessage("DiscountTypeIsNotValid");
    }
}