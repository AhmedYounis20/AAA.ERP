using Accounting.Application.Commands.Suppliers;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Domain.Account.Utility;
using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.Infrastructure.Services.Account.SubLeadgers.SubLeadgerBaseService;

namespace Accounting.Infrastructure.Services;

public class SupplierService : SubLeadgerService<Supplier, SupplierCreateCommand, SupplierUpdateCommand>, ISupplierService
{
    public SupplierService(IUnitOfWork unitOfWork, ISupplierRepository repository, IHttpContextAccessor accessor,
        IChartOfAccountRepository chartOfAccountRepository)
        : base(unitOfWork, repository, accessor, chartOfAccountRepository, SD.SupplierChartOfAccountId, SubLeadgerType.Supplier)
    {
    }
}
