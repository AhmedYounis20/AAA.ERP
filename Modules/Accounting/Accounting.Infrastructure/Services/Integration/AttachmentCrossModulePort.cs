using Accounting.Application.Repositories.Account;
using Accounting.Contracts.Integration;

namespace Accounting.Infrastructure.Services;

public class AttachmentCrossModulePort(IAttachmentRepository repository) : IAttachmentCrossModulePort
{
    public Task DeleteByIdsAsync(IReadOnlyCollection<Guid> attachmentIds, CancellationToken cancellationToken = default)
    {
        if (attachmentIds.Count == 0)
            return Task.CompletedTask;

        return repository.ExecuteDeleteAsync(e => attachmentIds.Contains(e.Id), cancellationToken);
    }

    public async Task DeleteIfExistsAsync(Guid attachmentId, CancellationToken cancellationToken = default)
    {
        var attachment = await repository.Get(attachmentId);
        if (attachment is not null)
            await repository.Delete(attachment);
    }
}
