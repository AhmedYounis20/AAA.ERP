using Accounting.Application.Commands.Taxes;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Infrastructure.Handlers.SubLedger;

namespace Accounting.Infrastructure.Handlers.Taxes;

public sealed class TaxCreateCommandHandler(
    ITaxRepository repository,
    ISubLedgerCoaParentResolver subLedgerCoaParentResolver,
    ISubLedgerCoaFactory subLedgerCoaFactory,
    ISubLedgerCoaParentValidator subLedgerCoaParentValidator)
    : BaseSubLedgerCreateHandler<Tax, TaxCreateCommand>(
        repository,
        subLedgerCoaParentResolver,
        subLedgerCoaFactory)
{
    protected override SubLeadgerType SubLedgerType => SubLeadgerType.Tax;

    protected override Task<ChartOfAccount> ResolveParentAccountAsync(
        TaxCreateCommand command,
        CancellationToken cancellationToken)
        => subLedgerCoaParentValidator.ValidateAndGetAsync(command.ChartOfAccountId, cancellationToken);
}
