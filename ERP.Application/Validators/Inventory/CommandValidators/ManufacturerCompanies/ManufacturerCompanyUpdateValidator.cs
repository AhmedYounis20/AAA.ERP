using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using ERP.Domain.Commands.Inventory.ManufacturerCompanies;
using ERP.Domain.Models.Entities.Inventory.ManufacturerCompanies;

namespace ERP.Application.Validators.Inventory.CommandValidators.ManufacturerCompanies;

public class ManufacturerCompanyUpdateValidator : BaseSettingUpdateValidator<ManufacturerCompanyUpdateCommand, ManufacturerCompany>
{ }