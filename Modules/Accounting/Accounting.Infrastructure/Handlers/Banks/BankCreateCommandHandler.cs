using Accounting.Application.Commands.Banks;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Utility;
using Accounting.Infrastructure.Handlers.SubLedger;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Persistence.Repositories;

namespace Accounting.Infrastructure.Handlers.Banks;

public sealed class BankCreateCommandHandler(
    IBankRepository repository,
    IChartOfAccountRepository chartOfAccountRepository)
    : BaseSubLedgerCreateHandler<Bank, BankCreateCommand>(
        repository,
        chartOfAccountRepository,
        Guid.Parse(SD.BankChartAccountId),
        SubLeadgerType.Bank);

public sealed class BankUpdateCommandHandler(IBankRepository repository)
    : BaseSubLedgerUpdateHandler<Bank, BankUpdateCommand>(repository);

public sealed class BankDeleteCommandHandler(
    IBankRepository repository,
    IUnitOfWork unitOfWork,
    IChartOfAccountRepository chartOfAccountRepository)
    : BaseSubLedgerDeleteHandler<Bank, BankDeleteCommand>(repository, unitOfWork, chartOfAccountRepository);
