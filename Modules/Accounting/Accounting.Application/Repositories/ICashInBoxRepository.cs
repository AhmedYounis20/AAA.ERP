using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Repositories;

/// <summary>
/// Repository contract for cash in box persistence.
/// </summary>
public interface ICashInBoxRepository : IBaseSubLeadgerRepository<CashInBox>;
