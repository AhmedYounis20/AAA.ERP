using ERP.Application.Repositories.SubLeadgers;
using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.Suppliers;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
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