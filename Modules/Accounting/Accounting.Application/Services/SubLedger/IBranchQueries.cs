using Accounting.Application.Commands.Branches;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Services.Queries;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for branch GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IBranchQueries : ISubLedgerQueries<Branch, BranchCreateCommand>;
