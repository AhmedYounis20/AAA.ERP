using Accounting.Application.Commands.Customers;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Customer reads. Transitional: delegates to <see cref="ICustomerService"/>.
/// </summary>
public class CustomerReadService(ICustomerService customerService) : ICustomerReadService
{
    public Task<ApiResponse<IEnumerable<Customer>>> GetAll()
        => customerService.ReadAll();

    public Task<ApiResponse<PaginatedResult<Customer>>> GetPaginated(
        SubLeadgerFilterDto filter,
        CancellationToken cancellationToken = default)
        => customerService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<Customer>> GetById(Guid id)
        => customerService.ReadById(id);

    public Task<ApiResponse<CustomerCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => customerService.GetNextSubLeadgers(parentId);

    public async Task<ApiResponse<IEnumerable<Customer>>> GetLevel(int level = 0)
    {
        var items = await customerService.GetLevel(level);
        return new ApiResponse<IEnumerable<Customer>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items
        };
    }

    public async Task<ApiResponse<IEnumerable<Customer>>> GetChildren(Guid parentId, int level = 0)
    {
        var items = await customerService.GetChildren(parentId, level);
        return new ApiResponse<IEnumerable<Customer>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items
        };
    }
}
