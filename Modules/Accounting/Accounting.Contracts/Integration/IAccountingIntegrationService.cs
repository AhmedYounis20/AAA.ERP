using Shared.Responses;

namespace Accounting.Contracts.Integration;

/// <summary>
/// Application service contract for accounting integration.
/// </summary>
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
