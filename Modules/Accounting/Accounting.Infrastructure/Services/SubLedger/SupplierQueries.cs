using Accounting.Application.Commands.Suppliers;
using Accounting.Application.Repositories;
using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Infrastructure.Services.SubLeadgers;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Supplier read queries.
/// </summary>
public class SupplierQueries(
    ISupplierRepository repository,
    IChartOfAccountRepository chartOfAccountRepository,
    ISubLedgerCoaParentResolver subLedgerCoaParentResolver)
    : BaseTreeSettingRepositoryQueries<Supplier>(repository),
      ISupplierQueries
{
    public async Task<ApiResponse<SupplierCreateCommand>> GetNextSubLedgers(Guid? parentId)
    {
        var coaParent = await subLedgerCoaParentResolver.ResolveAsync(SubLeadgerType.Supplier);
        return await SubLedgerNextDefaultsHelper.GetNextSubLedgersAsync<Supplier, SupplierCreateCommand>(
            chartOfAccountRepository,
            coaParent.Id,
            parentId);
    }
}
