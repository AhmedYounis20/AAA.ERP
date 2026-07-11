using Accounting.Application.Commands.CashInBoxes;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Utility;
using Accounting.Infrastructure.Handlers.SubLedger;
using ERP.BuildingBlocks.Persistence.Repositories;

namespace Accounting.Infrastructure.Handlers.CashInBoxes;

public sealed class CashInBoxCreateCommandHandler(
    ICashInBoxRepository repository,
    IChartOfAccountRepository chartOfAccountRepository)
    : BaseSubLedgerCreateHandler<CashInBox, CashInBoxCreateCommand>(
        repository,
        chartOfAccountRepository,
        Guid.Parse(Sd.CashInBoxChartOfAccountId),
        SubLeadgerType.CashInBox);

public sealed class CashInBoxUpdateCommandHandler(ICashInBoxRepository repository)
    : BaseSubLedgerUpdateHandler<CashInBox, CashInBoxUpdateCommand>(repository);

public sealed class CashInBoxDeleteCommandHandler(
    ICashInBoxRepository repository,
    IUnitOfWork unitOfWork,
    IChartOfAccountRepository chartOfAccountRepository)
    : BaseSubLedgerDeleteHandler<CashInBox, CashInBoxDeleteCommand>(repository, unitOfWork, chartOfAccountRepository);
