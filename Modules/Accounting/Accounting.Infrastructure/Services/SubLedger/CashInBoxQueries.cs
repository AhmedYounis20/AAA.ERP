using Accounting.Application.Commands.CashInBoxes;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Utility;
using Accounting.Infrastructure.Services.SubLeadgers;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Accounting.Infrastructure.Services;

public class CashInBoxQueries(ICashInBoxRepository repository, IChartOfAccountRepository chartOfAccountRepository)
    : BaseTreeSettingRepositoryQueries<CashInBox>(repository),
      ICashInBoxQueries
{
    public Task<ApiResponse<CashInBoxCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => SubLedgerNextDefaultsHelper.GetNextSubLedgersAsync<CashInBox, CashInBoxCreateCommand>(
            chartOfAccountRepository,
            Guid.Parse(Sd.CashInBoxChartOfAccountId),
            parentId);
}
