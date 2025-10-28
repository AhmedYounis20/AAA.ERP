using Inventory.Domain.Commands.ManufacturerCompanies;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;

namespace Inventory.Infrastructure.Handlers.ManufacturerCompanies;

public class ManufacturerCompanyUpdateCommandHandler(IManufacturerCompanyService service) : ICommandHandler<ManufacturerCompanyUpdateCommand, ApiResponse<ManufacturerCompany>>
{
    public async Task<ApiResponse<ManufacturerCompany>> Handle(ManufacturerCompanyUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}