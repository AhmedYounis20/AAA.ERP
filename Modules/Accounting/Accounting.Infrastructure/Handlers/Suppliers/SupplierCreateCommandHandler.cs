using Accounting.Application.Commands.Suppliers;
using Accounting.Application.Repositories;
using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Utility;
using Accounting.Infrastructure.Handlers.SubLedger;
using ERP.BuildingBlocks.Persistence.Repositories;

namespace Accounting.Infrastructure.Handlers.Suppliers;

public sealed class SupplierCreateCommandHandler(
    ISupplierRepository repository,
    IChartOfAccountRepository chartOfAccountRepository)
    : BaseSubLedgerCreateHandler<Supplier, SupplierCreateCommand>(
        repository,
        chartOfAccountRepository,
        Guid.Parse(SD.SupplierChartOfAccountId),
        SubLeadgerType.Supplier);

public sealed class SupplierUpdateCommandHandler(ISupplierRepository repository)
    : BaseSubLedgerUpdateHandler<Supplier, SupplierUpdateCommand>(repository);

public sealed class SupplierDeleteCommandHandler(
    ISupplierRepository repository,
    IUnitOfWork unitOfWork,
    IChartOfAccountRepository chartOfAccountRepository)
    : BaseSubLedgerDeleteHandler<Supplier, SupplierDeleteCommand>(repository, unitOfWork, chartOfAccountRepository);
