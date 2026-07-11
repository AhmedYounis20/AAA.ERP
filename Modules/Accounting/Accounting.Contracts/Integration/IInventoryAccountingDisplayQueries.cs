namespace Accounting.Contracts.Integration;

/// <summary>Display metadata for Accounting entities referenced by Inventory FK columns.</summary>
public record AccountingEntityDisplayInfo(
    Guid Id,
    string? Name,
    string? NameSecondLanguage,
    string? Code);

/// <summary>Batch read port for enriching Inventory API responses with branch and party names.</summary>
public interface IInventoryAccountingDisplayQueries
{
    Task<IReadOnlyDictionary<Guid, AccountingEntityDisplayInfo>> GetBranchesByIdsAsync(
        IReadOnlyCollection<Guid> branchIds,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<Guid, AccountingEntityDisplayInfo>> GetPartyAccountsByIdsAsync(
        IReadOnlyCollection<Guid> chartOfAccountIds,
        CancellationToken cancellationToken = default);
}
