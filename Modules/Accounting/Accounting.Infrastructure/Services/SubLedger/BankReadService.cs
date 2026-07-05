using Accounting.Application.Commands.Banks;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Bank reads. Transitional: delegates to <see cref="IBankService"/>.
/// </summary>
public class BankReadService(IBankService bankService) : IBankReadService
{
    public Task<ApiResponse<IEnumerable<Bank>>> GetAll()
        => bankService.ReadAll();

    public Task<ApiResponse<PaginatedResult<Bank>>> GetPaginated(
        SubLeadgerFilterDto filter,
        CancellationToken cancellationToken = default)
        => bankService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<Bank>> GetById(Guid id)
        => bankService.ReadById(id);

    public Task<ApiResponse<BankCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => bankService.GetNextSubLeadgers(parentId);

    public async Task<ApiResponse<IEnumerable<Bank>>> GetLevel(int level = 0)
    {
        var items = await bankService.GetLevel(level);
        return new ApiResponse<IEnumerable<Bank>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items
        };
    }

    public async Task<ApiResponse<IEnumerable<Bank>>> GetChildren(Guid parentId, int level = 0)
    {
        var items = await bankService.GetChildren(parentId, level);
        return new ApiResponse<IEnumerable<Bank>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items
        };
    }
}
