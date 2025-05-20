using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.ManufacturerCompanies;
using ERP.Domain.Models.Entities.Inventory.ManufacturerCompanies;

namespace ERP.Infrastracture.Handlers.Inventory.ManufacturerCompanies;

public class ManufacturerCompanyUpdateCommandHandler(IManufacturerCompanyService service) : ICommandHandler<ManufacturerCompanyUpdateCommand, ApiResponse<ManufacturerCompany>>
{
    public async Task<ApiResponse<ManufacturerCompany>> Handle(ManufacturerCompanyUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}