using Accounting.Application.Commands.CashInBoxes;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

public class CashInBoxQueries(ICashInBoxRepository repository, ICashInBoxService cashInBoxService)
    : BaseTreeSettingRepositoryQueries<CashInBox>(repository),
      ICashInBoxQueries
{
    public Task<ApiResponse<CashInBoxCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => cashInBoxService.GetNextSubLeadgers(parentId);
}
