using ERP.BuildingBlocks.Application.Validators.Account.CommandValidators.BaseCommandValidators.CreateCommandValidators;
using Inventory.Application.Commands.Sizes;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Application.Validators.Sizes;

public class SizeCreateValidator : BaseSettingCreateValidator<SizeCreateCommand, Size>;
