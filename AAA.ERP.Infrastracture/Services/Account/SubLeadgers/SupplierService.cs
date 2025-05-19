using Domain.Account.Commands.SubLeadgers.Suppliers;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Repositories.SubLeadgers;
using ERP.Application.Services.Account.SubLeadgers;
using ERP.Infrastracture.Services.Account.SubLeadgers.SubLeadgerBaseService;

namespace ERP.Infrastracture.Services.Account.SubLeadgers;

public class SupplierService : SubLeadgerService<Supplier, SupplierCreateCommand, SupplierUpdateCommand>,
    ISupplierService
{
    private IUnitOfWork _unitOfWork;
    private IHttpContextAccessor _accessor;

    public SupplierService(IUnitOfWork unitOfWork, ISupplierRepository repository, IHttpContextAccessor accessor)
        : base(unitOfWork, repository, accessor, SD.SupplierChartOfAccountId, SubLeadgerType.Supplier)
    {
        _unitOfWork = unitOfWork;
        _accessor = accessor;
    }
}