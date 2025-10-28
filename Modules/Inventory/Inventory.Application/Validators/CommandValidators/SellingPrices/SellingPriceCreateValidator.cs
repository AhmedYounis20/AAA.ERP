using Inventory.Domain.Commands.SellingPrices;
using Inventory.Domain.Models.Entities.SellingPrices;
using Shared.Application.Validators.BaseCommandValidators.CreateCommandValidators;

namespace Inventory.Application.Validators.CommandValidators.SellingPrices;

public class SellingPriceCreateValidator : BaseSettingCreateValidator<SellingPriceCreateCommand, SellingPrice>
{ }