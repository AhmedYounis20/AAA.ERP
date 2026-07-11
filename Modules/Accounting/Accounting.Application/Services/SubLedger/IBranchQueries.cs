using Accounting.Application.Commands.Branches;
using Accounting.Application.Services.SubLedger.Base;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for branch GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IBranchQueries : ISubLedgerQueries<Branch, BranchCreateCommand>;
