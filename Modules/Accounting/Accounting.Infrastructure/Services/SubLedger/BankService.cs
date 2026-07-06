using Accounting.Application.Commands.Banks;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Utility;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Bank default-data helper. CRUD writes are handled by CQRS handlers.
/// </summary>
public class BankService(IChartOfAccountRepository chartOfAccountRepository) : IBankService
{
    public Task<ApiResponse<BankCreateCommand>> GetNextSubLeadgers(Guid? parentId)
        => SubLeadgers.SubLedgerNextDefaultsHelper.GetNextSubLedgersAsync<Bank, BankCreateCommand>(
            chartOfAccountRepository,
            Guid.Parse(SD.BankChartAccountId),
            parentId);
}
