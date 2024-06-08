using Domain.Account.Commands.SubLeadgers.Customers;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Repositories.Interfaces.SubLeadgers;
using Domain.Account.Services.Impelementation.SubLeadgers.SubLeadgerBaseService;
using Domain.Account.Services.Interfaces.SubLeadgers;
using Domain.Account.Utility;
using Microsoft.AspNetCore.Http;

namespace Domain.Account.Services.Impelementation.SubLeadgers;

public class CustomerService : SubLeadgerService<Customer, CustomerCreateCommand,CustomerUpdateCommand>,
    ICustomerService
{
    private IUnitOfWork _unitOfWork;
    private IHttpContextAccessor _accessor;

    public CustomerService(IUnitOfWork unitOfWork,ICustomerRepository repository, IHttpContextAccessor accessor)
        : base(unitOfWork, repository, accessor, SD.CustomerChartOfAccountId)
    {
        _unitOfWork = unitOfWork;
        _accessor = accessor;
    }
}