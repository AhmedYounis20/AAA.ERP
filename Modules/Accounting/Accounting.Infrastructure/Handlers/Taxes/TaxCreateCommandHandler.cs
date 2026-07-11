using Accounting.Application.Commands.Taxes;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Infrastructure.Handlers.SubLedger;

namespace Accounting.Infrastructure.Handlers.Taxes;

public sealed class TaxCreateCommandHandler(
    ITaxRepository repository,
    IChartOfAccountRepository chartOfAccountRepository)
    : BaseSubLedgerCreateHandler<Tax, TaxCreateCommand>(
        repository,
        chartOfAccountRepository,
        Guid.Empty,
        SubLeadgerType.Tax)
{
    protected override Guid ResolveChartOfAccountParentId(TaxCreateCommand command)
        => command.ChartOfAccountId;
}
