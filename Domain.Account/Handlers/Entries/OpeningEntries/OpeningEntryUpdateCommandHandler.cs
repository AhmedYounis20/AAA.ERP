using Domain.Account.Commands.Entries.OpeningEntries;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Services.Interfaces.Entries;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Entries.OpeningEntries;

public class OpeningEntryUpdateCommandHandler(IOpeningEntryService openingEntryService) : ICommandHandler<OpeningEntryUpdateCommand, ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(OpeningEntryUpdateCommand request, CancellationToken cancellationToken)
    {
        return await openingEntryService.Update(request);
    }
}