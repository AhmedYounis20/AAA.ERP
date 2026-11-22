using Accounting.Application.Commands.CashInBoxes;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Infrastructure.Handlers.SubLedger;
using ERP.BuildingBlocks.Persistence.Repositories;

namespace Accounting.Infrastructure.Handlers.CashInBoxes;

public sealed class CashInBoxCreateCommandHandler(
    ICashInBoxRepository repository,
    ISubLedgerCoaParentResolver subLedgerCoaParentResolver,
    ISubLedgerCoaFactory subLedgerCoaFactory)
    : BaseSubLedgerCreateHandler<CashInBox, CashInBoxCreateCommand>(
        repository,
        subLedgerCoaParentResolver,
        subLedgerCoaFactory)
{
    protected override SubLeadgerType SubLedgerType => SubLeadgerType.CashInBox;
}

public sealed class CashInBoxUpdateCommandHandler(ICashInBoxRepository repository)
    : BaseSubLedgerUpdateHandler<CashInBox, CashInBoxUpdateCommand>(repository);

public sealed class CashInBoxDeleteCommandHandler(
    ICashInBoxRepository repository,
    IUnitOfWork unitOfWork,
    IChartOfAccountRepository chartOfAccountRepository)
    : BaseSubLedgerDeleteHandler<CashInBox, CashInBoxDeleteCommand>(repository, unitOfWork, chartOfAccountRepository);
