using Accounting.Application.Commands.Banks;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Infrastructure.Handlers.SubLedger;
using ERP.BuildingBlocks.Persistence.Repositories;

namespace Accounting.Infrastructure.Handlers.Banks;

public sealed class BankCreateCommandHandler(
    IBankRepository repository,
    ISubLedgerCoaParentResolver subLedgerCoaParentResolver,
    ISubLedgerCoaFactory subLedgerCoaFactory)
    : BaseSubLedgerCreateHandler<Bank, BankCreateCommand>(
        repository,
        subLedgerCoaParentResolver,
        subLedgerCoaFactory)
{
    protected override SubLeadgerType SubLedgerType => SubLeadgerType.Bank;
}

public sealed class BankUpdateCommandHandler(IBankRepository repository)
    : BaseSubLedgerUpdateHandler<Bank, BankUpdateCommand>(repository);

public sealed class BankDeleteCommandHandler(
    IBankRepository repository,
    IUnitOfWork unitOfWork,
    IChartOfAccountRepository chartOfAccountRepository)
    : BaseSubLedgerDeleteHandler<Bank, BankDeleteCommand>(repository, unitOfWork, chartOfAccountRepository);
