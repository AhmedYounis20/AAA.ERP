using Inventory.Application.Commands.ManufacturerCompanies;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;

namespace Inventory.Infrastructure.Handlers.ManufacturerCompanies;

public class ManufacturerCompanyCreateCommandHandler(IManufacturerCompanyService service)
    : ICommandHandler<ManufacturerCompanyCreateCommand, ApiResponse<ManufacturerCompany>>
{
    public Task<ApiResponse<ManufacturerCompany>> Handle(ManufacturerCompanyCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
