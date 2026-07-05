using Accounting.Application.Commands.Customers;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Domain.Account.Utility;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
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
