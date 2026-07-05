using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Manufacturer company read queries. Transitional: delegates to <see cref="IManufacturerCompanyService"/> until write methods are removed.
/// </summary>
public class ManufacturerCompanyReadService(IManufacturerCompanyService manufacturerCompanyService) : IManufacturerCompanyReadService
{
    public Task<ApiResponse<IEnumerable<ManufacturerCompany>>> GetAll()
        => manufacturerCompanyService.ReadAll();

    public Task<ApiResponse<PaginatedResult<ManufacturerCompany>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default)
        => manufacturerCompanyService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<ManufacturerCompany>> GetById(Guid id)
        => manufacturerCompanyService.ReadById(id);
}
