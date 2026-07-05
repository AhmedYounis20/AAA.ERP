using Accounting.Application.Commands.CashInBoxes;
using Accounting.Application.Dtos.Filters;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

public interface ICashInBoxReadService
{
    Task<ApiResponse<IEnumerable<CashInBox>>> GetAll();
    Task<ApiResponse<PaginatedResult<CashInBox>>> GetPaginated(SubLeadgerFilterDto filter, CancellationToken cancellationToken = default);
    Task<ApiResponse<CashInBox>> GetById(Guid id);
    Task<ApiResponse<CashInBoxCreateCommand>> GetNextSubLedgers(Guid? parentId);
    Task<ApiResponse<IEnumerable<CashInBox>>> GetLevel(int level = 0);
    Task<ApiResponse<IEnumerable<CashInBox>>> GetChildren(Guid parentId, int level = 0);
}
