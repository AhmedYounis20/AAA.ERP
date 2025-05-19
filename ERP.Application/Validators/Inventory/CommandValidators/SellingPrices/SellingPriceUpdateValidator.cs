using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using ERP.Domain.Commands.Inventory.SellingPrices;
using ERP.Domain.Models.Entities.Inventory.SellingPrices;

namespace ERP.Application.Validators.Inventory.CommandValidators.SellingPrices;

public class SellingPriceUpdateValidator : BaseSettingUpdateValidator<SellingPriceUpdateCommand, SellingPrice>
{ }