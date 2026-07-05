using Accounting.Application.Commands.Customers;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Utility;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.Infrastructure.Services.Account.SubLeadgers.SubLeadgerBaseService;

namespace Accounting.Infrastructure.Services;

public class CustomerService : SubLeadgerService<Customer, CustomerCreateCommand, CustomerUpdateCommand>, ICustomerService
{
    public CustomerService(IUnitOfWork unitOfWork, ICustomerRepository repository, IHttpContextAccessor accessor,
        IChartOfAccountRepository chartOfAccountRepository)
        : base(unitOfWork, repository, accessor, chartOfAccountRepository, SD.CustomerChartOfAccountId, SubLeadgerType.Customer)
    {
    }
}
