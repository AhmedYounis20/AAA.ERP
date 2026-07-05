using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Inventory.Application.Commands.SellingPrices;
using Inventory.Domain.Models.Entities.SellingPrices;

namespace Inventory.Application.Validators.SellingPrices;

public class SellingPriceUpdateValidator : BaseSettingUpdateValidator<SellingPriceUpdateCommand, SellingPrice>;
