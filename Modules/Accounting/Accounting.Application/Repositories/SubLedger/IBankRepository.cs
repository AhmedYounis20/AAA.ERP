using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Repositories;

/// <summary>
/// Repository contract for bank persistence.
/// </summary>
public interface IBankRepository : IBaseSubLeadgerRepository<Bank>;
