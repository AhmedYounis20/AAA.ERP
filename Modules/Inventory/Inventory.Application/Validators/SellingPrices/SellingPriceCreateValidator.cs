using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Inventory.Application.Commands.SellingPrices;
using ERP.Domain.Models.Entities.Inventory.SellingPrices;

namespace Inventory.Application.Validators.SellingPrices;

public class SellingPriceCreateValidator : BaseSettingCreateValidator<SellingPriceCreateCommand, SellingPrice>;
