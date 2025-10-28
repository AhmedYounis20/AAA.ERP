using Inventory.Domain.Commands.ManufacturerCompanies;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;

namespace Inventory.Infrastructure.Handlers.ManufacturerCompanies;

public class ManufacturerCompanyCreateCommandHandler(IManufacturerCompanyService service) : ICommandHandler<ManufacturerCompanyCreateCommand, ApiResponse<ManufacturerCompany>>
{
    public async Task<ApiResponse<ManufacturerCompany>> Handle(ManufacturerCompanyCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}