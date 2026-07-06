using Accounting.Application.Commands.Banks;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Bank read queries.
/// </summary>
public class BankQueries(IBankRepository repository, IBankService bankService)
    : BaseTreeSettingRepositoryQueries<Bank>(repository),
      IBankQueries
{
    public Task<ApiResponse<BankCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => bankService.GetNextSubLeadgers(parentId);
}
