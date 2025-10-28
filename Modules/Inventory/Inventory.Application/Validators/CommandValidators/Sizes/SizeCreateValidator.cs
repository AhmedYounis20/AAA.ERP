using FluentValidation;
using Inventory.Domain.Commands.Sizes;
using Inventory.Domain.Models.Entities.Sizes;
using Shared.Application.Validators.BaseCommandValidators.CreateCommandValidators;

namespace Inventory.Application.Validators.CommandValidators.Sizes;

public class SizeCreateValidator : BaseSettingCreateValidator<SizeCreateCommand, Size>{}