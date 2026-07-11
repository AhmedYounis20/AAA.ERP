using Accounting.Application.Commands.Customers;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Utility;
using Accounting.Infrastructure.Handlers.SubLedger;
using ERP.BuildingBlocks.Persistence.Repositories;

namespace Accounting.Infrastructure.Handlers.Customers;

public sealed class CustomerCreateCommandHandler(
    ICustomerRepository repository,
    IChartOfAccountRepository chartOfAccountRepository)
    : BaseSubLedgerCreateHandler<Customer, CustomerCreateCommand>(
        repository,
        chartOfAccountRepository,
        Guid.Parse(Sd.CustomerChartOfAccountId),
        SubLeadgerType.Customer);

public sealed class CustomerUpdateCommandHandler(ICustomerRepository repository)
    : BaseSubLedgerUpdateHandler<Customer, CustomerUpdateCommand>(repository);

public sealed class CustomerDeleteCommandHandler(
    ICustomerRepository repository,
    IUnitOfWork unitOfWork,
    IChartOfAccountRepository chartOfAccountRepository)
    : BaseSubLedgerDeleteHandler<Customer, CustomerDeleteCommand>(repository, unitOfWork, chartOfAccountRepository);
