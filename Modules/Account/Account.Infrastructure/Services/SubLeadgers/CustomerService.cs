using Account.Application.Repositories.SubLeadgers;
using Account.Domain.Commands.SubLeadgers.Customers;
using Account.Domain.Models.Entities.ChartOfAccounts;

namespace Account.Infrastructure.Services.SubLeadgers;

public class CustomerService : SubLeadgerService<Customer, CustomerCreateCommand, CustomerUpdateCommand>,
    ICustomerService
{
    private IUnitOfWork _unitOfWork;

    public CustomerService(IUnitOfWork unitOfWork, ICustomerRepository repository)
        : base(unitOfWork, repository, SD.CustomerChartOfAccountId, SubLeadgerType.Customer)
    {
        _unitOfWork = unitOfWork;
    }
}
