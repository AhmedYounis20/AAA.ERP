using Inventory.Domain.Commands.SellingPrices;
using Inventory.Domain.Models.Entities.SellingPrices;
using Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;

namespace Inventory.Application.Validators.CommandValidators.SellingPrices;

public class SellingPriceUpdateValidator : BaseSettingUpdateValidator<SellingPriceUpdateCommand, SellingPrice>
{ }