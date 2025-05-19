using ERP.Application.Repositories.Account.SubLeadgers;
using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.Customers;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Infrastracture.Services.Account.SubLeadgers.SubLeadgerBaseService;

namespace ERP.Infrastracture.Services.Account.SubLeadgers;

public class CustomerService : SubLeadgerService<Customer, CustomerCreateCommand, CustomerUpdateCommand>,
    ICustomerService
{
    private IUnitOfWork _unitOfWork;
    private IHttpContextAccessor _accessor;

    public CustomerService(IUnitOfWork unitOfWork, ICustomerRepository repository, IHttpContextAccessor accessor)
        : base(unitOfWork, repository, accessor, SD.CustomerChartOfAccountId, SubLeadgerType.Customer)
    {
        _unitOfWork = unitOfWork;
        _accessor = accessor;
    }
}