using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Inventory.Application.Commands.SellingPrices;
using ERP.Domain.Models.Entities.Inventory.SellingPrices;

namespace Inventory.Application.Validators.SellingPrices;

public class SellingPriceUpdateValidator : BaseSettingUpdateValidator<SellingPriceUpdateCommand, SellingPrice>;
