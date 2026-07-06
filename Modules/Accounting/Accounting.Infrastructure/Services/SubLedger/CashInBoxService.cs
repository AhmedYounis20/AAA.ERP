using Accounting.Application.Commands.CashInBoxes;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Utility;

namespace Accounting.Infrastructure.Services;

public class CashInBoxService(IChartOfAccountRepository chartOfAccountRepository) : ICashInBoxService
{
    public Task<ApiResponse<CashInBoxCreateCommand>> GetNextSubLeadgers(Guid? parentId)
        => SubLeadgers.SubLedgerNextDefaultsHelper.GetNextSubLedgersAsync<CashInBox, CashInBoxCreateCommand>(
            chartOfAccountRepository,
            Guid.Parse(SD.CashInBoxChartOfAccountId),
            parentId);
}
