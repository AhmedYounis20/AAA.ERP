using ERP.BuildingBlocks.Application.Validators.Account.CommandValidators.BaseCommandValidators.CreateCommandValidators;
using Inventory.Application.Commands.SellingPrices;
using Inventory.Domain.Models.Entities.SellingPrices;

namespace Inventory.Application.Validators.SellingPrices;

public class SellingPriceCreateValidator : BaseSettingCreateValidator<SellingPriceCreateCommand, SellingPrice>;
