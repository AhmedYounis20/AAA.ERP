using Accounting.Application.Commands.Taxes;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Infrastructure.Handlers.SubLedger;
using ERP.BuildingBlocks.Persistence.Repositories;

namespace Accounting.Infrastructure.Handlers.Taxes;

public sealed class TaxUpdateCommandHandler(ITaxRepository repository)
    : BaseSubLedgerUpdateHandler<Tax, TaxUpdateCommand>(repository);
