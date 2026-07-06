using Accounting.Application.Commands.Taxes;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Account.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Tax read queries. Transitional: delegates to <see cref="ITaxService"/> until write methods are removed.
/// </summary>
public class TaxQueries(ITaxService taxService)
    : BaseTreeSettingQueries<Tax, TaxCreateCommand, TaxUpdateCommand>(taxService),
      ITaxQueries
{
    public Task<ApiResponse<TaxCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => taxService.GetNextSubLeadgers(parentId);
}
