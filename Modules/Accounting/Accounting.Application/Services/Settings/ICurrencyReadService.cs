using Accounting.Application.Dtos.Filters;
using Accounting.Domain.Models.Entities.Currencies;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

public interface ICurrencyReadService
{
    Task<ApiResponse<IEnumerable<Currency>>> GetAll();
    Task<ApiResponse<PaginatedResult<Currency>>> GetPaginated(SettingFilterDto filter, CancellationToken cancellationToken = default);
    Task<ApiResponse<Currency>> GetById(Guid id);
}
