using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Inventory.Application.Commands.PackingUnits;
using Inventory.Domain.Models.Entities.PackingUnits;

namespace Inventory.Application.Validators.PackingUnits;

public class PackingUnitCreateValidator : BaseSettingCreateValidator<PackingUnitCreateCommand, PackingUnit>;
