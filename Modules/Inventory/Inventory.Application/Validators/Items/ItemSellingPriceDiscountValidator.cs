using Inventory.Domain.Models.Dtos;
using Inventory.Domain.Models.Entities.Items;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Application.Validators.Items;

public class ItemSellingPriceDiscountValidator : AbstractValidator<ItemSellingPriceDiscountDto>
{
    public ItemSellingPriceDiscountValidator()
    {

        _ = RuleFor(e => e.SellingPriceId).NotEmpty().WithMessage(ValidationMessages.SellingPriceIsRequired);
        _ = RuleFor(e => e.Discount).GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.DiscountCannotBeNegative);
        _ = RuleFor(e => e.DiscountType).IsInEnum().WithMessage(ValidationMessages.DiscountTypeIsNotValid);
    }
}