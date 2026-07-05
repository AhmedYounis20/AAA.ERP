using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Inventory.Application.Commands.PackingUnits;
using Inventory.Domain.Models.Entities.PackingUnits;

namespace Inventory.Application.Validators.PackingUnits;

public class PackingUnitUpdateValidator : BaseSettingUpdateValidator<PackingUnitUpdateCommand, PackingUnit>;
