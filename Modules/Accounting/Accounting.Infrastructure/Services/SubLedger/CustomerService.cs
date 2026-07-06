using Accounting.Application.Commands.Customers;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Utility;

namespace Accounting.Infrastructure.Services;

public class CustomerService(IChartOfAccountRepository chartOfAccountRepository) : ICustomerService
{
    public Task<ApiResponse<CustomerCreateCommand>> GetNextSubLeadgers(Guid? parentId)
        => SubLeadgers.SubLedgerNextDefaultsHelper.GetNextSubLedgersAsync<Customer, CustomerCreateCommand>(
            chartOfAccountRepository,
            Guid.Parse(SD.CustomerChartOfAccountId),
            parentId);
}
