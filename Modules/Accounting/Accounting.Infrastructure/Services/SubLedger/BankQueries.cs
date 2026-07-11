using Accounting.Application.Commands.Banks;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Utility;
using Accounting.Infrastructure.Services.SubLeadgers;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Bank read queries.
/// </summary>
public class BankQueries(IBankRepository repository, IChartOfAccountRepository chartOfAccountRepository)
    : BaseTreeSettingRepositoryQueries<Bank>(repository),
      IBankQueries
{
    public Task<ApiResponse<BankCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => SubLedgerNextDefaultsHelper.GetNextSubLedgersAsync<Bank, BankCreateCommand>(
            chartOfAccountRepository,
            Guid.Parse(Sd.BankChartAccountId),
            parentId);
}
