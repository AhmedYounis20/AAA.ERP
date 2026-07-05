using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Inventory.Application.Commands.Sizes;
using ERP.Domain.Models.Entities.Inventory.Sizes;

namespace Inventory.Application.Validators.Sizes;

public class SizeCreateValidator : BaseSettingCreateValidator<SizeCreateCommand, Size>;
