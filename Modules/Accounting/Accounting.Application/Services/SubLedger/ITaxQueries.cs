using Accounting.Application.Commands.Taxes;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Services.Queries;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for tax GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface ITaxQueries : ISubLedgerQueries<Tax, TaxCreateCommand>;
