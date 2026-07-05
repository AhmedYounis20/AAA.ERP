using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Inventory.Application.Commands.Items;
using Inventory.Domain.Models.Entities.Items;
using FluentValidation;
using Shared.BaseEntities;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Application.Validators.Items;

public class ItemCreateValidator : BaseTreeSettingCreateValidator<ItemCreateCommand, Item>
{ 
    public ItemCreateValidator() {

        _ = RuleFor(e => e.Code).NotEmpty().WithMessage(ValidationMessages.CodeIsRequired);
        _ = RuleFor(e => e.EGSCode).Must(e=> string.IsNullOrEmpty(e)).When(e=> e.NodeType == NodeType.Domain && !string.IsNullOrEmpty(e.Gs1Code)).WithMessage(ValidationMessages.ONLY_ONE_IS_NEEDED_EGS_OR_GS1);
        _ = RuleFor(e => e.PackingUnits).NotEmpty().When(e => e.NodeType == NodeType.Domain).WithMessage(ValidationMessages.PackingUnitsIsRequired);
        _ = RuleForEach(e => e.SellingPriceDiscounts).SetValidator(new ItemSellingPriceDiscountValidator()).When(e => e.NodeType == NodeType.Domain);
        _ = RuleForEach(e => e.PackingUnits).SetValidator(new ItemPackingUnitValidator()).When(e => e.NodeType == NodeType.Domain);
    }
    
}