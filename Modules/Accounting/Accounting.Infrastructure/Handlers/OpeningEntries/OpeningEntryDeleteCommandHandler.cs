using Accounting.Application.Commands.OpeningEntries;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Handlers.OpeningEntries;

public class OpeningEntryDeleteCommandHandler(IOpeningEntryService openingEntryService)
    : ICommandHandler<OpeningEntryDeleteCommand, ApiResponse<Entry>>
{
    public Task<ApiResponse<Entry>> Handle(OpeningEntryDeleteCommand request, CancellationToken cancellationToken)
        => openingEntryService.Delete(request.Id);
}
