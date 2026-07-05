using Accounting.Application.Commands.Banks;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Bank read queries. Transitional: delegates to <see cref="IBankService"/>.
/// </summary>
public class BankQueries(IBankService bankService)
    : BaseTreeSettingQueries<Bank, BankCreateCommand, BankUpdateCommand>(bankService),
      IBankQueries
{
    public Task<ApiResponse<BankCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => bankService.GetNextSubLeadgers(parentId);
}
