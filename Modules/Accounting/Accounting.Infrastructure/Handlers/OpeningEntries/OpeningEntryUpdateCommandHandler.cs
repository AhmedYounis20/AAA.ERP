using Accounting.Application.Services;
using Accounting.Application.Commands.OpeningEntries;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Handlers.OpeningEntries;

public class OpeningEntryUpdateCommandHandler(IOpeningEntryService openingEntryService) : ICommandHandler<OpeningEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(OpeningEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await openingEntryService.Update(request);
    }
}