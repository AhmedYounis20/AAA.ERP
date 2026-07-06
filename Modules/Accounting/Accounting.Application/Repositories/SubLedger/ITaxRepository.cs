using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Application.Repositories;

/// <summary>
/// Repository contract for tax persistence.
/// </summary>
public interface ITaxRepository : IBaseSubLeadgerRepository<Tax>;
