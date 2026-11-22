using Accounting.Application.Commands.Customers;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Infrastructure.Services.SubLeadgers;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Customer read queries.
/// </summary>
public class CustomerQueries(
    ICustomerRepository repository,
    IChartOfAccountRepository chartOfAccountRepository,
    ISubLedgerCoaParentResolver subLedgerCoaParentResolver)
    : BaseTreeSettingRepositoryQueries<Customer>(repository),
      ICustomerQueries
{
    public async Task<ApiResponse<CustomerCreateCommand>> GetNextSubLedgers(Guid? parentId)
    {
        var coaParent = await subLedgerCoaParentResolver.ResolveAsync(SubLeadgerType.Customer);
        return await SubLedgerNextDefaultsHelper.GetNextSubLedgersAsync<Customer, CustomerCreateCommand>(
            chartOfAccountRepository,
            coaParent.Id,
            parentId);
    }
}
