using Accounting.Application.Commands.Taxes;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.Account.SubLeadgers;
using Accounting.Infrastructure.Handlers.SubLedger;
using ERP.BuildingBlocks.Persistence.Repositories;

namespace Accounting.Infrastructure.Handlers.Taxes;

public sealed class TaxDeleteCommandHandler(
    ITaxRepository repository,
    IUnitOfWork unitOfWork,
    IChartOfAccountRepository chartOfAccountRepository)
    : BaseSubLedgerDeleteHandler<Tax, TaxDeleteCommand>(repository, unitOfWork, chartOfAccountRepository);
