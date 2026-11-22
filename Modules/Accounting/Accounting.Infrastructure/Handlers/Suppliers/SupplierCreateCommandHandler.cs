using Accounting.Application.Commands.Suppliers;
using Accounting.Application.Repositories;
using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Infrastructure.Handlers.SubLedger;
using ERP.BuildingBlocks.Persistence.Repositories;

namespace Accounting.Infrastructure.Handlers.Suppliers;

public sealed class SupplierCreateCommandHandler(
    ISupplierRepository repository,
    ISubLedgerCoaParentResolver subLedgerCoaParentResolver,
    ISubLedgerCoaFactory subLedgerCoaFactory)
    : BaseSubLedgerCreateHandler<Supplier, SupplierCreateCommand>(
        repository,
        subLedgerCoaParentResolver,
        subLedgerCoaFactory)
{
    protected override SubLeadgerType SubLedgerType => SubLeadgerType.Supplier;
}

public sealed class SupplierUpdateCommandHandler(ISupplierRepository repository)
    : BaseSubLedgerUpdateHandler<Supplier, SupplierUpdateCommand>(repository);

public sealed class SupplierDeleteCommandHandler(
    ISupplierRepository repository,
    IUnitOfWork unitOfWork,
    IChartOfAccountRepository chartOfAccountRepository)
    : BaseSubLedgerDeleteHandler<Supplier, SupplierDeleteCommand>(repository, unitOfWork, chartOfAccountRepository);
