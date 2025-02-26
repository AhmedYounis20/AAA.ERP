using Domain.Account.Commands.SubLeadgers.Customers;
using Domain.Account.Commands.SubLeadgers.Suppliers;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Repositories.Interfaces.SubLeadgers;
using Domain.Account.Services.Impelementation.SubLeadgers.SubLeadgerBaseService;
using Domain.Account.Services.Interfaces.SubLeadgers;
using Domain.Account.Utility;
using Microsoft.AspNetCore.Http;

namespace Domain.Account.Services.Impelementation.SubLeadgers;

public class SupplierService : SubLeadgerService<Supplier, SupplierCreateCommand,SupplierUpdateCommand>,
    ISupplierService
{
    private IUnitOfWork _unitOfWork;
    private IHttpContextAccessor _accessor;

    public SupplierService(IUnitOfWork unitOfWork,ISupplierRepository repository, IHttpContextAccessor accessor)
        : base(unitOfWork, repository, accessor, SD.SupplierChartOfAccountId,SubLeadgerType.Supplier)
    {
        _unitOfWork = unitOfWork;
        _accessor = accessor;
    }
}