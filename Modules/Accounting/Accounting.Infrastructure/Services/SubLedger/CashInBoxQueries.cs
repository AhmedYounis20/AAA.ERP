using Accounting.Application.Commands.CashInBoxes;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Cash-in-box read queries. Transitional: delegates to <see cref="ICashInBoxService"/>.
/// </summary>
public class CashInBoxQueries(ICashInBoxService cashInBoxService)
    : BaseTreeSettingQueries<CashInBox, CashInBoxCreateCommand, CashInBoxUpdateCommand>(cashInBoxService),
      ICashInBoxQueries
{
    public Task<ApiResponse<CashInBoxCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => cashInBoxService.GetNextSubLeadgers(parentId);
}
