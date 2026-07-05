using Accounting.Application.Commands.Taxes;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Domain.Entities;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Tax read queries. Transitional: delegates to <see cref="ITaxService"/> until write methods are removed from that type.
/// </summary>
public class TaxReadService(ITaxService taxService) : ITaxReadService
{
    public Task<ApiResponse<IEnumerable<Tax>>> GetAll()
        => taxService.ReadAll();

    public Task<ApiResponse<PaginatedResult<Tax>>> GetPaginated(
        SubLeadgerFilterDto filter,
        CancellationToken cancellationToken = default)
        => taxService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<Tax>> GetById(Guid id)
        => taxService.ReadById(id);

    public Task<ApiResponse<TaxCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => taxService.GetNextSubLeadgers(parentId);

    public async Task<ApiResponse<IEnumerable<Tax>>> GetLevel(int level = 0)
    {
        var items = await taxService.GetLevel(level);
        return new ApiResponse<IEnumerable<Tax>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items
        };
    }

    public async Task<ApiResponse<IEnumerable<Tax>>> GetChildren(Guid parentId, int level = 0)
    {
        var items = await taxService.GetChildren(parentId, level);
        return new ApiResponse<IEnumerable<Tax>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items
        };
    }
}
