using Shared.Responses;

namespace ERP.BuildingBlocks.Persistence.Repositories;

/// <summary>
/// Coordinates cross-module ACID operations (Phase 4 — MODULAR_MONOLITH.md §6.4).
/// </summary>
public interface ITransactionCoordinator
{
    Task<ApiResponse<T>> ExecuteAsync<T>(
        Func<CancellationToken, Task<ApiResponse<T>>> action,
        CancellationToken cancellationToken = default);
}
