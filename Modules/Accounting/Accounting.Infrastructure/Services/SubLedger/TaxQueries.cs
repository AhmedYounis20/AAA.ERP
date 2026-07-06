using Accounting.Application.Commands.Taxes;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Tax read queries.
/// </summary>
public class TaxQueries(ITaxRepository repository, ITaxService taxService)
    : BaseTreeSettingRepositoryQueries<Tax>(repository),
      ITaxQueries
{
    public Task<ApiResponse<TaxCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => taxService.GetNextSubLeadgers(parentId);
}
