using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for manufacturer company GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IManufacturerCompanyReadService
{
    Task<ApiResponse<IEnumerable<ManufacturerCompany>>> GetAll();

    Task<ApiResponse<PaginatedResult<ManufacturerCompany>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<ManufacturerCompany>> GetById(Guid id);
}
