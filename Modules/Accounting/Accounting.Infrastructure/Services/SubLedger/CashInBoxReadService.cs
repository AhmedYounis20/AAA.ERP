using Accounting.Application.Commands.CashInBoxes;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Infrastructure.Services;

public class CashInBoxReadService(ICashInBoxService cashInBoxService) : ICashInBoxReadService
{
    public Task<ApiResponse<IEnumerable<CashInBox>>> GetAll() => cashInBoxService.ReadAll();
    public Task<ApiResponse<PaginatedResult<CashInBox>>> GetPaginated(SubLeadgerFilterDto filter, CancellationToken cancellationToken = default)
        => cashInBoxService.ReadAllPaginated(filter, cancellationToken);
    public Task<ApiResponse<CashInBox>> GetById(Guid id) => cashInBoxService.ReadById(id);
    public Task<ApiResponse<CashInBoxCreateCommand>> GetNextSubLedgers(Guid? parentId) => cashInBoxService.GetNextSubLeadgers(parentId);

    public async Task<ApiResponse<IEnumerable<CashInBox>>> GetLevel(int level = 0)
    {
        var items = await cashInBoxService.GetLevel(level);
        return new ApiResponse<IEnumerable<CashInBox>> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = items };
    }

    public async Task<ApiResponse<IEnumerable<CashInBox>>> GetChildren(Guid parentId, int level = 0)
    {
        var items = await cashInBoxService.GetChildren(parentId, level);
        return new ApiResponse<IEnumerable<CashInBox>> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = items };
    }
}
