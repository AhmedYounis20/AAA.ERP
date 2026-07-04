using Shared.Responses;

namespace Accounting.Contracts.Integration;

public interface IAccountingIntegrationService
{
    Task<ApiResponse<FinancialPeriodInfo>> GetOpenPeriodForDateAsync(
        DateTime date,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<BranchAccountInfo>> GetBranchAccountAsync(
        Guid branchId,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<Guid>> ValidatePartyAccountAsync(
        Guid chartOfAccountId,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<Guid>> PostInventoryTransactionEntryAsync(
        InventoryTransactionPostingRequest request,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<Guid>> PostInventoryTransferEntryAsync(
        InventoryTransferPostingRequest request,
        CancellationToken cancellationToken = default);
}
