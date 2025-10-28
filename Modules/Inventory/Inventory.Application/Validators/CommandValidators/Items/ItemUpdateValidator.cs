using FluentValidation;
using Inventory.Domain.Commands.Items;
using Inventory.Domain.Models.Entities.Items;
using Shared.Domain.BaseEntities;
using Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;

namespace Inventory.Application.Validators.CommandValidators.Items;

public class ItemUpdateValidator : BaseTreeSettingUpdateValidator<ItemUpdateCommand, Item>
{
    public ItemUpdateValidator()
    {
        //_ = RuleFor(e => e.Code).NotEmpty().WithMessage("CodeIsRequired");
        //_ = RuleFor(e => e.EGSCode).Must(e => string.IsNullOrEmpty(e)).When(e => e.NodeType == NodeType.Domain && !string.IsNullOrEmpty(e.Gs1Code)).WithMessage("ONLY_ONE_IS_NEEDED_EGS_OR_GS1");
        _ = RuleFor(e => e.PackingUnits).NotEmpty().When(e => e.NodeType == NodeType.Domain).WithMessage("PackingUnitsIsRequired");
        _ = RuleForEach(e => e.SellingPriceDiscounts).SetValidator(new ItemSellingPriceDiscountValidator()).When(e => e.NodeType == NodeType.Domain);
        _ = RuleForEach(e => e.PackingUnits).SetValidator(new ItemPackingUnitValidator()).When(e => e.NodeType == NodeType.Domain);

    }
}