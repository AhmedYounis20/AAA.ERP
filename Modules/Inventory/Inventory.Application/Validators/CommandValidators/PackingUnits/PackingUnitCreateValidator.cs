using Inventory.Domain.Commands.PackingUnits;
using Inventory.Domain.Models.Entities.PackingUnits;
using Shared.Application.Validators.BaseCommandValidators.CreateCommandValidators;

namespace Inventory.Application.Validators.CommandValidators.PackingUnits;

public class PackingUnitCreateValidator : BaseSettingCreateValidator<PackingUnitCreateCommand, PackingUnit>
{ }