using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Repositories;

/// <summary>
/// Repository contract for branch persistence.
/// </summary>
public interface IBranchRepository : IBaseSubLeadgerRepository<Branch>;
