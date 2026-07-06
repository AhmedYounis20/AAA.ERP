using Inventory.Application.Commands.ManufacturerCompanies;
using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;

namespace Inventory.Infrastructure.Handlers.ManufacturerCompanies;

public sealed class ManufacturerCompanyDeleteCommandHandler(IManufacturerCompanyRepository repository)
    : BaseSettingDeleteHandler<ManufacturerCompany, ManufacturerCompanyDeleteCommand>(repository);
