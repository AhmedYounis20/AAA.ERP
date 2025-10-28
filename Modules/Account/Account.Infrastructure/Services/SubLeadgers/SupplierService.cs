using Account.Application.Repositories.SubLeadgers;
using Account.Domain.Commands.SubLeadgers.Suppliers;
using Account.Domain.Models.Entities.ChartOfAccounts;

namespace Account.Infrastructure.Services.SubLeadgers;

public class SupplierService : SubLeadgerService<Supplier, SupplierCreateCommand, SupplierUpdateCommand>,
    ISupplierService
{
    private IUnitOfWork _unitOfWork;

    public SupplierService(IUnitOfWork unitOfWork, ISupplierRepository repository)
        : base(unitOfWork, repository, SD.SupplierChartOfAccountId, SubLeadgerType.Supplier)
    {
        _unitOfWork = unitOfWork;
    }
}
