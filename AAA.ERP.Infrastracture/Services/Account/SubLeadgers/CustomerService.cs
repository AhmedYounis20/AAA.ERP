using Domain.Account.Commands.SubLeadgers.Customers;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Repositories.SubLeadgers;
using ERP.Application.Services.Account.SubLeadgers;
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