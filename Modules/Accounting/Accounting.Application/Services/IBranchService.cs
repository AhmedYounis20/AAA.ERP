using Accounting.Application.Commands.Branches;
using Accounting.Application.Services.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for branch.
/// </summary>
public interface IBranchService : IBaseSubLeadgerService<Branch, BranchCreateCommand, BranchUpdateCommand>;
