using Accounting.Application.Commands.Customers;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Customer read queries. Transitional: delegates to <see cref="ICustomerService"/>.
/// </summary>
public class CustomerQueries(ICustomerService customerService)
    : BaseTreeSettingQueries<Customer, CustomerCreateCommand, CustomerUpdateCommand>(customerService),
      ICustomerQueries
{
    public Task<ApiResponse<CustomerCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => customerService.GetNextSubLeadgers(parentId);
}
