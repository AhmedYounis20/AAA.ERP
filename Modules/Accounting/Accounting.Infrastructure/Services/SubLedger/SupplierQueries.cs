using Accounting.Application.Commands.Suppliers;
using Accounting.Application.Repositories;
using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Utility;
using Accounting.Infrastructure.Services.SubLeadgers;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Accounting.Infrastructure.Services;

public class SupplierQueries(ISupplierRepository repository, IChartOfAccountRepository chartOfAccountRepository)
    : BaseTreeSettingRepositoryQueries<Supplier>(repository),
      ISupplierQueries
{
    public Task<ApiResponse<SupplierCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => SubLedgerNextDefaultsHelper.GetNextSubLedgersAsync<Supplier, SupplierCreateCommand>(
            chartOfAccountRepository,
            Guid.Parse(Sd.SupplierChartOfAccountId),
            parentId);
}
