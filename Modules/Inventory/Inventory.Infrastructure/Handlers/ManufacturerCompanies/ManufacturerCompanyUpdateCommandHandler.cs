using Inventory.Application.Commands.ManufacturerCompanies;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;

namespace Inventory.Infrastructure.Handlers.ManufacturerCompanies;

public class ManufacturerCompanyUpdateCommandHandler(IManufacturerCompanyService service)
    : ICommandHandler<ManufacturerCompanyUpdateCommand, ApiResponse<ManufacturerCompany>>
{
    public Task<ApiResponse<ManufacturerCompany>> Handle(ManufacturerCompanyUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
