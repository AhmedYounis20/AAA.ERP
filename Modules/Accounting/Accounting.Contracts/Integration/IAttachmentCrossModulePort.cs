namespace Accounting.Contracts.Integration;

/// <summary>
/// Cross-module attachment operations for Inventory catalog writes (no Accounting persistence types on the wire).
/// </summary>
public interface IAttachmentCrossModulePort
{
    Task DeleteByIdsAsync(IReadOnlyCollection<Guid> attachmentIds, CancellationToken cancellationToken = default);

    Task DeleteIfExistsAsync(Guid attachmentId, CancellationToken cancellationToken = default);
}
