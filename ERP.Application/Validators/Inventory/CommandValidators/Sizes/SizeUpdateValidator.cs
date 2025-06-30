using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using ERP.Domain.Commands.Inventory.Sizes;
using ERP.Domain.Models.Entities.Inventory.Sizes;
using FluentValidation;

namespace ERP.Application.Validators.Inventory.CommandValidators.Sizes;

public class SizeUpdateValidator : BaseSettingUpdateValidator<SizeUpdateCommand, Size>
{ }