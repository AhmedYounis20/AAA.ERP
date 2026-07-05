using Accounting.Application.Commands.Customers;
using Accounting.Application.Dtos.Filters;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for customer GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface ICustomerReadService
{
    Task<ApiResponse<IEnumerable<Customer>>> GetAll();

    Task<ApiResponse<PaginatedResult<Customer>>> GetPaginated(
        SubLeadgerFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<Customer>> GetById(Guid id);

    Task<ApiResponse<CustomerCreateCommand>> GetNextSubLedgers(Guid? parentId);

    Task<ApiResponse<IEnumerable<Customer>>> GetLevel(int level = 0);

    Task<ApiResponse<IEnumerable<Customer>>> GetChildren(Guid parentId, int level = 0);
}
