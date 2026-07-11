using ERP.BuildingBlocks.Application.Validators.Account.CommandValidators.BaseCommandValidators.CreateCommandValidators;
using Inventory.Application.Commands.PackingUnits;
using Inventory.Domain.Models.Entities.PackingUnits;

namespace Inventory.Application.Validators.PackingUnits;

public class PackingUnitCreateValidator : BaseSettingCreateValidator<PackingUnitCreateCommand, PackingUnit>;
