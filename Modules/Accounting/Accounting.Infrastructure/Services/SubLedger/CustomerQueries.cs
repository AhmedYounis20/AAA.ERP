using Accounting.Application.Commands.Customers;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

public class CustomerQueries(ICustomerRepository repository, ICustomerService customerService)
    : BaseTreeSettingRepositoryQueries<Customer>(repository),
      ICustomerQueries
{
    public Task<ApiResponse<CustomerCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => customerService.GetNextSubLeadgers(parentId);
}
