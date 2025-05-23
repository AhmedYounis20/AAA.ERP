using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using ERP.Domain.Commands.Inventory.Items;
using ERP.Domain.Models.Entities.Inventory.Items;
using FluentValidation;

namespace ERP.Application.Validators.Inventory.CommandValidators.SellingPrices;

public class ItemUpdateValidator : BaseTreeSettingUpdateValidator<ItemUpdateCommand, Item>
{
    public ItemUpdateValidator()
    {
        _ = RuleFor(e => e.Code).NotEmpty().WithMessage("CodeIsRequired");
    }
}