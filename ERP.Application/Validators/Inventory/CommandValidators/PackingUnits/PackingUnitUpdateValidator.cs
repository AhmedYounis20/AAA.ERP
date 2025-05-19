using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using ERP.Domain.Commands.Inventory.PackingUnits;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;

namespace ERP.Application.Validators.Inventory.CommandValidators.PackingUnits;

public class PackingUnitUpdateValidator : BaseSettingUpdateValidator<PackingUnitUpdateCommand, PackingUnit>
{ }