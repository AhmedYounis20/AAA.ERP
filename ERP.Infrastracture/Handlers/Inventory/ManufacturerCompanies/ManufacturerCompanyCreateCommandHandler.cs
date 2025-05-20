using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.ManufacturerCompanies;
using ERP.Domain.Models.Entities.Inventory.ManufacturerCompanies;

namespace ERP.Infrastracture.Handlers.Inventory.ManufacturerCompanies;

public class ManufacturerCompanyCreateCommandHandler(IManufacturerCompanyService service) : ICommandHandler<ManufacturerCompanyCreateCommand, ApiResponse<ManufacturerCompany>>
{
    public async Task<ApiResponse<ManufacturerCompany>> Handle(ManufacturerCompanyCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}