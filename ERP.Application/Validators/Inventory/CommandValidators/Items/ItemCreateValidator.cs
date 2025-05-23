using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using ERP.Domain.Commands.Inventory.Items;
using ERP.Domain.Models.Entities.Inventory.Items;
using FluentValidation;

namespace ERP.Application.Validators.Inventory.CommandValidators.Items;

public class ItemCreateValidator : BaseTreeSettingCreateValidator<ItemCreateCommand, Item>
{ 
    public ItemCreateValidator() {

        _ = RuleFor(e => e.Code).NotEmpty().WithMessage("CodeIsRequired");
    }
    
}