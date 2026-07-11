using Accounting.Application.Commands.Customers;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Utility;
using Accounting.Infrastructure.Services.SubLeadgers;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Accounting.Infrastructure.Services;

public class CustomerQueries(ICustomerRepository repository, IChartOfAccountRepository chartOfAccountRepository)
    : BaseTreeSettingRepositoryQueries<Customer>(repository),
      ICustomerQueries
{
    public Task<ApiResponse<CustomerCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => SubLedgerNextDefaultsHelper.GetNextSubLedgersAsync<Customer, CustomerCreateCommand>(
            chartOfAccountRepository,
            Guid.Parse(Sd.CustomerChartOfAccountId),
            parentId);
}
