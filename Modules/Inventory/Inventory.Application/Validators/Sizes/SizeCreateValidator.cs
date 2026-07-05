using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Inventory.Application.Commands.Sizes;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Application.Validators.Sizes;

public class SizeCreateValidator : BaseSettingCreateValidator<SizeCreateCommand, Size>;
