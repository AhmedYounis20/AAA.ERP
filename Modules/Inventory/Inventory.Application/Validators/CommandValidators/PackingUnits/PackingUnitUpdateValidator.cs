using Inventory.Domain.Commands.PackingUnits;
using Inventory.Domain.Models.Entities.PackingUnits;
using Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;

namespace Inventory.Application.Validators.CommandValidators.PackingUnits;

public class PackingUnitUpdateValidator : BaseSettingUpdateValidator<PackingUnitUpdateCommand, PackingUnit>
{ }