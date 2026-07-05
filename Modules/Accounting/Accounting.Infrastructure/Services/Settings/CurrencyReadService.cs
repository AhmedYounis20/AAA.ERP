using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Currencies;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Infrastructure.Services;

public class CurrencyReadService(ICurrencyService currencyService) : ICurrencyReadService
{
    public Task<ApiResponse<IEnumerable<Currency>>> GetAll() => currencyService.ReadAll();
    public Task<ApiResponse<PaginatedResult<Currency>>> GetPaginated(SettingFilterDto filter, CancellationToken cancellationToken = default)
        => currencyService.ReadAllPaginated(filter, cancellationToken);
    public Task<ApiResponse<Currency>> GetById(Guid id) => currencyService.ReadById(id);
}
