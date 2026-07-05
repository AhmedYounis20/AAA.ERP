using Inventory.Application.Commands.ManufacturerCompanies;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;

namespace Inventory.Infrastructure.Handlers.ManufacturerCompanies;

public class ManufacturerCompanyDeleteCommandHandler(IManufacturerCompanyService manufacturerCompanyService)
    : ICommandHandler<ManufacturerCompanyDeleteCommand, ApiResponse<ManufacturerCompany>>
{
    public Task<ApiResponse<ManufacturerCompany>> Handle(ManufacturerCompanyDeleteCommand request, CancellationToken cancellationToken)
        => manufacturerCompanyService.Delete(request.Id);
}
