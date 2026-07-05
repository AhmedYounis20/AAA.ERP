using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Inventory.Application.Commands.PackingUnits;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;

namespace Inventory.Application.Validators.PackingUnits;

public class PackingUnitCreateValidator : BaseSettingCreateValidator<PackingUnitCreateCommand, PackingUnit>;
