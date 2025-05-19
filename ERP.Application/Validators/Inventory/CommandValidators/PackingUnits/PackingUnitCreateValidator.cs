using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using ERP.Domain.Commands.Inventory.PackingUnits;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;

namespace ERP.Application.Validators.Inventory.CommandValidators.PackingUnits;

public class PackingUnitCreateValidator : BaseSettingCreateValidator<PackingUnitCreateCommand, PackingUnit>
{ }