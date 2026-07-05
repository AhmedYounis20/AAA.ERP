using ERP.BuildingBlocks.Application.Validators.Account.CommandValidators.BaseCommandValidators.UpdateCommandValidators;
using Inventory.Application.Commands.SellingPrices;
using Inventory.Domain.Models.Entities.SellingPrices;

namespace Inventory.Application.Validators.SellingPrices;

public class SellingPriceUpdateValidator : BaseSettingUpdateValidator<SellingPriceUpdateCommand, SellingPrice>;
