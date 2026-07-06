using Accounting.Application.Commands.Customers;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Customer default-data helper. CRUD writes are handled by CQRS handlers.
/// </summary>
public interface ICustomerService
{
    Task<ApiResponse<CustomerCreateCommand>> GetNextSubLeadgers(Guid? parentId);
}
