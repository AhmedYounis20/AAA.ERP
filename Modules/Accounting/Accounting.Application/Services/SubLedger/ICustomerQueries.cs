using Accounting.Application.Commands.Customers;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Application.Services.Queries;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for customer GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface ICustomerQueries : ITreeSettingQueries<Customer>
{
    Task<ApiResponse<CustomerCreateCommand>> GetNextSubLedgers(Guid? parentId);
}
