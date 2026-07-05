using Accounting.Application.Commands.CombinedEntries;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Handlers.CombinedEntries;

public class CombinedEntryDeleteCommandHandler(ICombinedEntryService combinedEntryService)
    : ICommandHandler<CombinedEntryDeleteCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(CombinedEntryDeleteCommand request, CancellationToken cancellationToken)
        => combinedEntryService.Delete(request.Id);
}
