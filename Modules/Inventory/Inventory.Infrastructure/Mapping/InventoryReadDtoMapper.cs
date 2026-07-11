using Accounting.Contracts.Integration;
using Inventory.Application.Dtos.Catalog;
using Inventory.Domain.Models.Entities;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using Inventory.Domain.Models.Entities.StockBalances;

namespace Inventory.Infrastructure.Mapping;

public static class InventoryReadDtoMapper
{
    public static async Task<InventoryTransactionReadDto> ToReadDtoAsync(
        InventoryTransaction transaction,
        IInventoryAccountingDisplayQueries displayQueries,
        CancellationToken cancellationToken = default)
    {
        var list = await ToReadDtosAsync([transaction], displayQueries, cancellationToken);
        return list[0];
    }

    public static async Task<IReadOnlyList<InventoryTransactionReadDto>> ToReadDtosAsync(
        IEnumerable<InventoryTransaction> transactions,
        IInventoryAccountingDisplayQueries displayQueries,
        CancellationToken cancellationToken = default)
    {
        var materialized = transactions.ToList();
        if (materialized.Count == 0)
            return [];

        var branchIds = materialized.Select(t => t.BranchId).Distinct().ToList();
        var partyIds = materialized.Select(t => t.TransactionPartyId).Distinct().ToList();
        var branches = await displayQueries.GetBranchesByIdsAsync(branchIds, cancellationToken);
        var parties = await displayQueries.GetPartyAccountsByIdsAsync(partyIds, cancellationToken);

        return materialized.Select(t => new InventoryTransactionReadDto
        {
            Id = t.Id,
            CreatedBy = t.CreatedBy,
            ModifiedBy = t.ModifiedBy,
            CreatedAt = t.CreatedAt,
            ModifiedAt = t.ModifiedAt,
            TransactionType = t.TransactionType,
            TransactionDate = t.TransactionDate,
            TransactionNumber = t.TransactionNumber,
            DocumentNumber = t.DocumentNumber,
            TransactionPartyId = t.TransactionPartyId,
            TransactionPartyName = parties.GetValueOrDefault(t.TransactionPartyId)?.Name,
            BranchId = t.BranchId,
            BranchName = branches.GetValueOrDefault(t.BranchId)?.Name,
            FinancialPeriodId = t.FinancialPeriodId,
            Notes = t.Notes,
            Items = t.Items
        }).ToList();
    }

    public static async Task<InventoryTransferReadDto> ToReadDtoAsync(
        InventoryTransfer transfer,
        IInventoryAccountingDisplayQueries displayQueries,
        CancellationToken cancellationToken = default)
    {
        var list = await ToReadDtosAsync([transfer], displayQueries, cancellationToken);
        return list[0];
    }

    public static async Task<IReadOnlyList<InventoryTransferReadDto>> ToReadDtosAsync(
        IEnumerable<InventoryTransfer> transfers,
        IInventoryAccountingDisplayQueries displayQueries,
        CancellationToken cancellationToken = default)
    {
        var materialized = transfers.ToList();
        if (materialized.Count == 0)
            return [];

        var branchIds = materialized
            .SelectMany(t => new[] { t.SourceBranchId, t.DestinationBranchId })
            .Distinct()
            .ToList();
        var branches = await displayQueries.GetBranchesByIdsAsync(branchIds, cancellationToken);

        return materialized.Select(t => new InventoryTransferReadDto
        {
            Id = t.Id,
            CreatedBy = t.CreatedBy,
            ModifiedBy = t.ModifiedBy,
            CreatedAt = t.CreatedAt,
            ModifiedAt = t.ModifiedAt,
            SourceBranchId = t.SourceBranchId,
            SourceBranchName = branches.GetValueOrDefault(t.SourceBranchId)?.Name,
            DestinationBranchId = t.DestinationBranchId,
            DestinationBranchName = branches.GetValueOrDefault(t.DestinationBranchId)?.Name,
            Status = t.Status,
            TransferType = t.TransferType,
            Notes = t.Notes,
            ApprovedBy = t.ApprovedBy,
            ApprovedAt = t.ApprovedAt,
            Items = t.Items
        }).ToList();
    }

    public static async Task<StockBalanceReadDto> ToReadDtoAsync(
        StockBalance balance,
        IInventoryAccountingDisplayQueries displayQueries,
        CancellationToken cancellationToken = default)
    {
        var list = await ToReadDtosAsync([balance], displayQueries, cancellationToken);
        return list[0];
    }

    public static async Task<IReadOnlyList<StockBalanceReadDto>> ToReadDtosAsync(
        IEnumerable<StockBalance> balances,
        IInventoryAccountingDisplayQueries displayQueries,
        CancellationToken cancellationToken = default)
    {
        var materialized = balances.ToList();
        if (materialized.Count == 0)
            return [];

        var branchIds = materialized.Select(b => b.BranchId).Distinct().ToList();
        var branches = await displayQueries.GetBranchesByIdsAsync(branchIds, cancellationToken);

        return materialized.Select(b => new StockBalanceReadDto
        {
            Id = b.Id,
            CreatedBy = b.CreatedBy,
            ModifiedBy = b.ModifiedBy,
            CreatedAt = b.CreatedAt,
            ModifiedAt = b.ModifiedAt,
            BranchId = b.BranchId,
            BranchName = branches.GetValueOrDefault(b.BranchId)?.Name,
            VariantId = b.VariantId,
            PackingUnitId = b.PackingUnitId,
            CurrentBalance = b.CurrentBalance,
            MinimumBalance = b.MinimumBalance,
            MaximumBalance = b.MaximumBalance,
            UnitCost = b.UnitCost,
            TotalCost = b.TotalCost
        }).ToList();
    }
}
