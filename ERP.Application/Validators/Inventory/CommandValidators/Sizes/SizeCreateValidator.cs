using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using ERP.Domain.Commands.Inventory.Sizes;
using ERP.Domain.Models.Entities.Inventory.Sizes;
using FluentValidation;

namespace ERP.Application.Validators.Inventory.CommandValidators.Sizes;

public class SizeCreateValidator : BaseSettingCreateValidator<SizeCreateCommand, Size>
{ }