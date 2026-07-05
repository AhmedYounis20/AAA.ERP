using Accounting.Application.Commands.Banks;
using Accounting.Application.Dtos.Filters;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for bank GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IBankReadService
{
    Task<ApiResponse<IEnumerable<Bank>>> GetAll();

    Task<ApiResponse<PaginatedResult<Bank>>> GetPaginated(
        SubLeadgerFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<Bank>> GetById(Guid id);

    Task<ApiResponse<BankCreateCommand>> GetNextSubLedgers(Guid? parentId);

    Task<ApiResponse<IEnumerable<Bank>>> GetLevel(int level = 0);

    Task<ApiResponse<IEnumerable<Bank>>> GetChildren(Guid parentId, int level = 0);
}
