using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Inventory.Application.Commands.PackingUnits;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;

namespace Inventory.Application.Validators.PackingUnits;

public class PackingUnitUpdateValidator : BaseSettingUpdateValidator<PackingUnitUpdateCommand, PackingUnit>;
